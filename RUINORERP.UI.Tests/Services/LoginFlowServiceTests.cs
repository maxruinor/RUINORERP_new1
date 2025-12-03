using RUINORERP.PacketSpec.Models.Authentication;
using RUINORERP.UI.Network.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Tests.Services
{
    /// <summary>
    /// 登录流程服务测试类
    /// </summary>
    [TestFixture]
    public class LoginFlowServiceTests
    {
        private Mock<IUserLoginService> _userLoginServiceMock;
        private Mock<ILogger<LoginFlowService>> _loggerMock;
        private LoginFlowService _loginFlowService;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _userLoginServiceMock = new Mock<IUserLoginService>();
            _loggerMock = new Mock<ILogger<LoginFlowService>>();
            _loginFlowService = new LoginFlowService(_userLoginServiceMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// 测试正常登录流程
        /// </summary>
        [Test]
        public async Task TestNormalLoginFlow()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var loginResponse = new LoginResponse
            {
                IsSuccess = true,
                Username = username,
                Message = "登录成功"
            };

            _userLoginServiceMock
                .Setup(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(loginResponse);

            // Act
            var result = await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(username, result.Username);
            Assert.AreEqual("登录成功", result.Message);
        }

        /// <summary>
        /// 测试重复登录时的选择功能
        /// </summary>
        [Test]
        public async Task TestDuplicateLoginSelection()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var sessionId = "session123";

            // 第一次登录响应：重复登录
            var duplicateLoginResponse = new LoginResponse
            {
                IsSuccess = false,
                Username = username,
                Message = "您的账号已在其他地方登录",
                Metadata = new Dictionary<string, object>
                {
                    {"ExistingSessions", new List<object>()}
                }
            };

            // 第二次登录响应：登录成功（用户选择强制下线）
            var successResponse = new LoginResponse
            {
                IsSuccess = true,
                Username = username,
                Message = "登录成功",
                SessionId = sessionId
            };

            _userLoginServiceMock
                .SetupSequence(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(duplicateLoginResponse)
                .ReturnsAsync(successResponse);

            // Act & Assert
            // 由于涉及UI对话框，这里主要测试服务端交互逻辑
            // 实际的对话框选择测试需要UI自动化测试框架
            Assert.DoesNotThrowAsync(async () =>
            {
                await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);
            });
        }

        /// <summary>
        /// 测试选择"踢掉其他设备"后的登录成功场景
        /// </summary>
        [Test]
        public async Task TestForceOfflineOthersSuccess()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var sessionId = "session123";

            // 模拟存在远程会话的重复登录响应
            var duplicateLoginResponse = new LoginResponse
            {
                IsSuccess = false,
                Username = username,
                Message = "您的账号已在其他地方登录",
                Metadata = new Dictionary<string, object>
                {
                    {"ExistingSessions", new List<object> {
                        new Dictionary<string, object> {
                            {"SessionId", "existingSession1"},
                            {"ClientIp", "192.168.1.100"},
                            {"IsLocal", false}
                        }
                    }}
                }
            };

            var successResponse = new LoginResponse
            {
                IsSuccess = true,
                Username = username,
                Message = "登录成功",
                SessionId = sessionId
            };

            _userLoginServiceMock
                .SetupSequence(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(duplicateLoginResponse)
                .ReturnsAsync(successResponse);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
            {
                await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);
            });
        }

        /// <summary>
        /// 测试选择"放弃登录"后的退出流程
        /// </summary>
        [Test]
        public async Task TestCancelLoginFlow()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";

            // 模拟重复登录响应
            var duplicateLoginResponse = new LoginResponse
            {
                IsSuccess = false,
                Username = username,
                Message = "您的账号已在其他地方登录",
                Metadata = new Dictionary<string, object>
                {
                    {"ExistingSessions", new List<object>()}
                }
            };

            _userLoginServiceMock
                .Setup(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(duplicateLoginResponse);

            // Act & Assert
            // 测试放弃登录的情况
            Assert.DoesNotThrowAsync(async () =>
            {
                await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);
            });
        }

        /// <summary>
        /// 测试网络异常情况下的错误处理
        /// </summary>
        [Test]
        public async Task TestNetworkExceptionHandling()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";

            // 模拟网络异常
            _userLoginServiceMock
                .Setup(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ThrowsAsync(new System.Net.Http.HttpRequestException("网络连接失败"));

            // Act & Assert
            // 验证网络异常被正确处理，不会导致程序崩溃
            Assert.DoesNotThrowAsync(async () =>
            {
                await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);
            });
        }

        /// <summary>
        /// 测试本地重复登录直接允许的场景
        /// </summary>
        [Test]
        public async Task TestLocalDuplicateLoginDirectAllow()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";

            // 模拟本地重复登录响应
            var duplicateLoginResponse = new LoginResponse
            {
                IsSuccess = false,
                Username = username,
                Message = "检测到本机已登录",
                Metadata = new Dictionary<string, object>
                {
                    {"ExistingSessions", new List<object> {
                        new Dictionary<string, object> {
                            {"SessionId", "existingSession1"},
                            {"ClientIp", "127.0.0.1"},
                            {"IsLocal", true}
                        }
                    }}
                }
            };

            _userLoginServiceMock
                .Setup(x => x.LoginAsync(username, password, It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(duplicateLoginResponse);

            // Act
            var result = await _loginFlowService.LoginAsync(username, password, "127.0.0.1", 8080);

            // Assert
            // 本地重复登录应该直接允许登录成功
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(username, result.Username);
        }

        // 注意：ExtractDuplicateLoginInfo是私有方法，不应该直接测试
        // 测试应该通过调用公开的API方法间接测试私有方法的功能
    }
}
