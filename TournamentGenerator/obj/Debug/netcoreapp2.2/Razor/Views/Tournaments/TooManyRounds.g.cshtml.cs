#pragma checksum "C:\Users\Stephen\source\repos\TournamentGenerator\TournamentGenerator\Views\Tournaments\TooManyRounds.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4571fece57c933a4ad77a294d9273d90cf496c28"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tournaments_TooManyRounds), @"mvc.1.0.view", @"/Views/Tournaments/TooManyRounds.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tournaments/TooManyRounds.cshtml", typeof(AspNetCore.Views_Tournaments_TooManyRounds))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Stephen\source\repos\TournamentGenerator\TournamentGenerator\Views\_ViewImports.cshtml"
using TournamentGenerator;

#line default
#line hidden
#line 2 "C:\Users\Stephen\source\repos\TournamentGenerator\TournamentGenerator\Views\_ViewImports.cshtml"
using TournamentGenerator.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4571fece57c933a4ad77a294d9273d90cf496c28", @"/Views/Tournaments/TooManyRounds.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f9befa9369539dfa2e5e6175e2bfa016ac102d8a", @"/Views/_ViewImports.cshtml")]
    public class Views_Tournaments_TooManyRounds : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 83, true);
            WriteLiteral("\r\n<div>You must have at least 2n - 1 players for a tournament with n rounds. </div>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
