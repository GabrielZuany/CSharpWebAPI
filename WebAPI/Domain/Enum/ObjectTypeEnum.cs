using System.ComponentModel;

namespace WebAPI.Domain.Enum
{
    public enum ObjectTypeEnum
    {
        [Description(".txt")]
        TXT,
        [Description(".json")]
        JSON,
        [Description(".png")]
        PNG,
    }
}
