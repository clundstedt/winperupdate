using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WinPerUpdateAdmin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/locales/bootstrap-datepicker.es.min.js"
                      ,"~/Scripts/jasny-bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/jasny-bootstrap.min.css"
                      , "~/Content/select.css",
                      "~/Content/Site.css").Include("~/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                              "~/Scripts/angular.min.js"
                            , "~/Scripts/angular-route.min.js"
                            , "~/Scripts/select.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                      "~/js/Login/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      ,"~/js/Login/login.js",
                      "~/js/Login/serviceLogin.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Scripts/angular-file-upload.min.js"
                      , "~/Scripts/smart-table.min.js"
                      , "~/js/Version/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/Version/admin.js"
                      , "~/js/Version/version.js"
                      , "~/js/Version/componente.js"
                      , "~/js/Version/editcomponente.js"
                      , "~/js/Version/publicar.js"
                      , "~/js/Version/serviceAdmin.js"
                      , "~/js/Clientes/serviceClientes.js"
                      , "~/js/Seguridad/serviceSeguridad.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/adminclt").Include(
                        "~/js/AdminClt/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/AdminClt/admin.js"
                      , "~/js/AdminClt/serviceAdmin.js"
                      , "~/js/AmbientesClt/serviceAmbientes.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/seguridaclt").Include(
                        "~/js/SeguridadClt/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/SeguridadClt/seguridad.js"
                      , "~/js/SeguridadClt/serviceSeguridad.js"
                      , "~/js/Clientes/serviceClientes.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/ambientesclt").Include(
                      "~/Scripts/angular-file-upload.min.js"
                      ,  "~/js/AmbientesClt/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/AmbientesClt/ambientes.js"
                      , "~/js/AmbientesClt/mantenedor.js"
                      , "~/js/SeguridadClt/serviceSeguridad.js"
                      , "~/js/AmbientesClt/serviceAmbientes.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/clientes").Include(
                       "~/Scripts/smart-table.min.js"
                      , "~/js/Clientes/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/Clientes/inicio.js"
                      , "~/js/Clientes/clientes.js"
                      , "~/js/Clientes/usuarios.js"
                      , "~/js/Clientes/serviceClientes.js"
                      , "~/js/Seguridad/serviceSeguridad.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/seguridad").Include(
                      "~/js/Seguridad/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/Seguridad/mantenedor.js"
                      , "~/js/Seguridad/seguridad.js"
                      , "~/js/Seguridad/serviceSeguridad.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/modulos").Include(
                        "~/Scripts/angular-file-upload.min.js"
                      , "~/Scripts/smart-table.min.js"
                      , "~/js/Modulos/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/Modulos/inicio.js"
                      , "~/js/Modulos/componentes.js"
                      , "~/js/Modulos/modulos.js"
                      , "~/js/Modulos/tipoComponente.js"
                      , "~/js/Modulos/serviceModulos.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/perfil").Include(
                      "~/js/Perfil/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/Perfil/perfil.js"
                      , "~/js/Seguridad/serviceSeguridad.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/superuser").Include(
                        "~/js/SuperUser/app.js"
                      , "~/js/Home/controllerHome.js"
                      , "~/js/Home/factoryHome.js"
                      , "~/js/SuperUser/superUser.js"
                      , "~/js/SuperUser/serviceSU.js"
                      ));
        }
    }
}