using TodoApp.Common;

namespace Test_TodoApp
{
    public class ErrorMessages_Test
    {
        [Theory]
        [InlineData(ErrorCode.TaskNotFound, "指定されたタスクの一部が見つかりませんでした")]
        [InlineData(ErrorCode.UpdateOrderFailed, "順序保存に失敗しました")]
        [InlineData(ErrorCode.UnknownError, "不明なエラーが発生しました")]
        public void エラーコードに対応するメッセージが正しく返される(ErrorCode code, string expectedMessage)
        {
            var errorMessage = ErrorMessages.Get(code);

            Assert.Equal(code, errorMessage.Code);
            Assert.Equal(expectedMessage, errorMessage.Message);

        }

        [Fact]
        public void 定義されていないエラーコードには未定義メッセージが返される()
        {
            var undefinedCode = (ErrorCode)999;
            var errorMessage = ErrorMessages.Get(undefinedCode);

            Assert.Equal(undefinedCode, errorMessage.Code);
            Assert.Equal("未定義のエラーです", errorMessage.Message);

        }
    }
}
