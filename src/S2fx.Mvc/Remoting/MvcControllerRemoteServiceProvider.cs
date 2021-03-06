using System;
using System.Collections.Generic;
using System.Text;
using S2fx.Remoting;
using S2fx.Remoting.Model;

namespace S2fx.Mvc.Remoting {

    public class MvcControllerRemoteServiceProvider : IRemoteServiceProvider {
        public string Name => "AspNetCoreController";

        public bool IsRemoteServiceProxyTypeRequired => true;

        public Type MakeRemoteServiceProxyType(RemoteServiceInfo service) {
            return service.ClrType;
        }
    }
}
