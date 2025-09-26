using Microsoft.VisualStudio.TestTools.UnitTesting;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Tests.Network.Commands
{
    [TestClass]
    public class SimplifiedLoginCommandHandlerTests
    {
        private SimplifiedLoginCommandHandler _handler;
        private TestCommand _testCommand;

        [TestInitialize]
        public async Task Setup()
        {
            _handler = new SimplifiedLoginCommandHandler();
            
            // 初始化处理器
            await _handler.InitializeAsync();
            await _handler.StartAsync();
            
            // 创建测试命令
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "testpassword",
                ClientVersion = "1.0.0",
                DeviceId = Guid.NewGuid().ToString(),
                LoginTime = DateTime.UtcNow,
                ClientType = "Desktop"
            };
            
            var packet = new PacketModel();
            packet.SetJsonData(loginRequest);
            
            _testCommand = new TestCommand
            {
                Packet = packet
            };
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _handler.StopAsync();
            _handler.Dispose();
        }

        [TestMethod]
        public async Task HandleAsync_ValidLoginRequest_ReturnsSuccessResponse()
        {
            // Act
            var response = await _handler.HandleAsync(_testCommand, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(200, response.Code);
        }

        [TestMethod]
        public async Task HandleAsync_ValidLoginRequest_ReturnsLoginResponseData()
        {
            // Arrange
            var testableHandler = _handler as ITestableCommandHandler;
            
            // Act
            var response = await testableHandler.HandleAsync(_testCommand, CancellationToken.None, false, 50);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            
            // 检查是否包含用户数据
            var apiResponse = response as ApiResponse<LoginResponse>;
            Assert.IsNotNull(apiResponse);
            Assert.IsNotNull(apiResponse.Data);
            Assert.AreEqual("testuser", apiResponse.Data.Username);
            Assert.IsTrue(apiResponse.Data.UserId > 0);
            Assert.IsFalse(string.IsNullOrEmpty(apiResponse.Data.SessionId));
        }

        [TestMethod]
        public async Task HandleAsync_SimulateFailure_ReturnsErrorResponse()
        {
            // Arrange
            var testableHandler = _handler as ITestableCommandHandler;
            
            // 启用模拟模式并设置失败概率
            var testConfig = testableHandler.GetTestConfiguration();
            testConfig.IsSimulationMode = true;
            testConfig.FailureProbability = 1.0; // 100%失败
            testableHandler.SetTestConfiguration(testConfig);

            // Act
            var response = await testableHandler.HandleAsync(_testCommand, CancellationToken.None, true, 0);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(500, response.Code);
            Assert.IsTrue(response.Message.Contains("模拟处理失败"));
        }

        [TestMethod]
        public async Task HandleAsync_WithDelay_SimulatesProcessingTime()
        {
            // Arrange
            var testableHandler = _handler as ITestableCommandHandler;
            
            // 启用模拟模式
            var testConfig = testableHandler.GetTestConfiguration();
            testConfig.IsSimulationMode = true;
            testableHandler.SetTestConfiguration(testConfig);

            var startTime = DateTime.UtcNow;

            // Act
            var response = await testableHandler.HandleAsync(_testCommand, CancellationToken.None, false, 100);

            var endTime = DateTime.UtcNow;
            var processingTime = (endTime - startTime).TotalMilliseconds;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
            Assert.IsTrue(processingTime >= 100, "处理时间应大于等于100毫秒");
        }

        [TestMethod]
        public async Task GetStatistics_ReturnsValidStatistics()
        {
            // Act
            var statistics = _handler.GetStatistics();

            // Assert
            Assert.IsNotNull(statistics);
            Assert.IsTrue(statistics.StartTime <= DateTime.UtcNow);
            Assert.AreEqual(0, statistics.TotalCommandsProcessed); // 还没有处理任何命令
        }

        [TestMethod]
        public async Task HandleAsync_UpdatesStatistics()
        {
            // Arrange
            var initialStats = _handler.GetStatistics();
            
            // Act
            await _handler.HandleAsync(_testCommand, CancellationToken.None);
            
            // Assert
            var updatedStats = _handler.GetStatistics();
            Assert.AreEqual(initialStats.TotalCommandsProcessed + 1, updatedStats.TotalCommandsProcessed);
            Assert.AreEqual(initialStats.SuccessfulCommands + 1, updatedStats.SuccessfulCommands);
            Assert.IsTrue(updatedStats.LastProcessTime >= initialStats.LastProcessTime);
        }

        [TestMethod]
        public async Task HandleAsync_InvalidRequest_ThrowsException()
        {
            // Arrange
            var invalidRequest = new LoginRequest
            {
                Username = "", // 空用户名
                Password = ""  // 空密码
            };
            
            var packet = new PacketModel();
            packet.SetJsonData(invalidRequest);
            
            var invalidCommand = new TestCommand
            {
                Packet = packet
            };

            // Act
            var response = await _handler.HandleAsync(invalidCommand, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(500, response.Code);
            Assert.IsTrue(response.Message.Contains("用户名和密码不能为空"));
        }
    }

    // 简单的测试命令实现
    public class TestCommand : ICommand
    {
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CommandId CommandIdentifier { get; set; } = new CommandId(CommandCategory.Authentication, 1);
        public CommandDirection Direction { get; set; } = CommandDirection.Send;
        public CommandPriority Priority { get; set; } = CommandPriority.Normal;
        public CommandStatus Status { get; set; } = CommandStatus.Created;
        public PacketModel Packet { get; set; }
        public int TimeoutMs { get; set; } = 30000;
        public string SessionID { get; set; }
        
        public CommandValidationResult Validate()
        {
            return CommandValidationResult.Success();
        }
        
        public object GetSerializableData()
        {
            return Packet?.GetJsonData<object>();
        }
    }
}