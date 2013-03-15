

namespace NFSLibrary.Protocols.V4.RPC
{
    using org.acplt.oncrpc;
    public class auth_flavor
    {
         public const int AUTH_NONE       = 0;
         public const int AUTH_SYS        = 1;
         public const int AUTH_SHORT      = 2;
         //added laterz
         public const int RPCSEC_GSS = 6;
    }
}
