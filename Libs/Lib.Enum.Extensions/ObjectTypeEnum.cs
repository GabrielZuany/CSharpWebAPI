using System.ComponentModel;

namespace Libs.Lib.Enum
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
