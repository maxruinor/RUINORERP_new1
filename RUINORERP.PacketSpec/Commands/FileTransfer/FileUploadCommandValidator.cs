using RUINORERP.PacketSpec.Validation;
using FluentValidation;
namespace RUINORERP.PacketSpec.Commands.FileTransfer
{
    /// <summary>
    /// 文件上传命令验证器
    /// </summary>
    public class FileUploadCommandValidator : CommandValidator<FileUploadCommand>
    {
        public FileUploadCommandValidator()
        {
            // 文件上传命令的验证规则
            RuleFor(command => command.FileName)
                .NotEmpty()
                .WithMessage("文件名不能为空");

            RuleFor(command => command.FileContentReader)
                .NotNull()
                .WithMessage("文件内容读取器不能为空");

            RuleFor(command => command.FileSize)
                .GreaterThan(0L)
                .WithMessage("文件大小无效");
        }
    }
}
