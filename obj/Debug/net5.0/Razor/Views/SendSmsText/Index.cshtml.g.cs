#pragma checksum "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\SendSmsText\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "73f66ad3941720da35b15d2b7d47ec2c0574d525"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SendSmsText_Index), @"mvc.1.0.view", @"/Views/SendSmsText/Index.cshtml")]
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
#nullable restore
#line 1 "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\_ViewImports.cshtml"
using SMSControlPanel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\_ViewImports.cshtml"
using SMSControlPanel.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\SendSmsText\Index.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"73f66ad3941720da35b15d2b7d47ec2c0574d525", @"/Views/SendSmsText/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"01b75087a9474518f696955e4a1841ddf84375cf", @"/Views/_ViewImports.cshtml")]
    public class Views_SendSmsText_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SMSControlPanel.Models.UserDetails>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/SendSmsText.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/Sendsmstext.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "0", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("image"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/btnSendMessage.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("send sms"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onclick", new global::Microsoft.AspNetCore.Html.HtmlString("SendMessage(event)"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "5", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("SMSForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\SendSmsText\Index.cshtml"
  
    var userviewmodel = JsonConvert.SerializeObject(Model);

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "73f66ad3941720da35b15d2b7d47ec2c0574d5257363", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script type=\"text/javascript\">\r\n        function getUser() {\r\n            var user = ");
#nullable restore
#line 12 "D:\Manju\SMS Control Panel_New\SMSControlPanel\Views\SendSmsText\Index.cshtml"
                  Write(Html.Raw(userviewmodel));

#line default
#line hidden
#nullable disable
                WriteLiteral(";\r\n            return user;\r\n        }\r\n    </script>\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73f66ad3941720da35b15d2b7d47ec2c0574d5258983", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            WriteLiteral(@"
<div id=""Content2"">
    <div>
        <table  cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" style=""width:100%;"">
            <tr>
                <td class=""smshead"" align=""center"" colspan=""4"">Send a Text Message</td>
            </tr>

            <tr>
                <td align=""left"" valign=""top"" style=""padding:12px; font-size:14px""><strong>All fields are mandatory</strong></td>
            </tr>
        </table>
        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73f66ad3941720da35b15d2b7d47ec2c0574d52510613", async() => {
                WriteLiteral(@"
            <table id=""table_content"" style=""width:100%;"" cellspacing=""20"" cellpadding=""0"" border=""0"" align=""center"">
                <tr id=""originator"" style=""vertical-align:top"">
                    <td align=""left"" valign=""top"" class=""auto-style3"">
                        <strong>Originator</strong>
                    </td>
                    <td align=""left"" valign=""top"">
                        <input id=""TxtOriginator"" name=""TxtOriginator"" type=""text"" maxlength=""11"" class=""textmedium"" style=""width: 250px; height: 20px"" />
                        <label id=""lbldisplaymsg"" style=""width:300px;font-size:smaller;"">This will appear on the recipient's phone as the sender of the message.</label>
                    </td>
                </tr>
                <tr id=""phoneNo"" style=""vertical-align:top"">
                    <td align=""left"" valign=""top"" class=""auto-style16"">
                        <strong>Phone No(s) :</strong>
                    </td>
                    <td align=""left"" val");
                WriteLiteral(@"ign=""top"" class=""auto-style17"">
                        <textarea id=""txtPhoneNumber"" name=""txtPhoneNumber""  class=""phnnotextarea"" rows=""2"" cols=""20"" style=""width: 250px;""></textarea>
                        <label id=""lblerrtxtphonenumber"" style=""margin-left:0.5rem;width:300px;font-size:smaller;"">Enter one or more phone numbers separated by commas or carriage returns.</label>
                    </td>
                </tr>
                <tr id=""ddrteamrow"" style=""vertical-align:top"">
                    <td align=""left"" colspan=""1"" class=""auto-style3"">
                        <label id=""Team"" class=""label"">Team</label>
                    </td>
                    <td align=""left"">
                        <select id=""ddrTeam"" name=""ddrTeam"" style=""width: 250px; height: 40px;"" class=""dropdown"" onchange=""OnTeamchange()"">
                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73f66ad3941720da35b15d2b7d47ec2c0574d52512902", async() => {
                    WriteLiteral("--Select Team--");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                        </select>
                    </td>
                </tr>
                <tr id=""ddrtitle"" style=""vertical-align:top"">
                    <td align=""left"" colspan=""1"" class=""auto-style14"">
                        <label id=""MessageTitle"" class=""label"">MessageTitle</label>
                    </td>
                    <td align=""left"" class=""auto-style15"">
                        <select id=""ddrMsgTitle"" name=""ddrMsgTitle"" style=""width: 250px; height: 40px; margin-bottom:10px;"" class=""dropdown"" disabled onchange=""OnMsgTitleChange()"">
                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73f66ad3941720da35b15d2b7d47ec2c0574d52514764", async() => {
                    WriteLiteral("--Select Message Title--");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                        </select>
                    </td>
                </tr>

                <tr id=""ddrmsg"" style=""vertical-align:top"">
                    <td align=""left"" class=""auto-style3"">
                        <strong>SMS Message</strong>
                    </td>
                    <td align=""left"">
                        <textarea id=""txtMessage"" name=""txtMessage"" maxlength=""400"" class=""smstextarea"" rows=""4"" cols=""3""></textarea>
                        &nbsp;&nbsp;
                        <label id=""lblsmsmsg"" class=""error-Labels"" hidden>No message provided</label>
                    </td>
                </tr>

                <tr id=""ddrmsgnopopup"" style=""vertical-align:top"">
                    <td align=""left"" class=""auto-style3"">
                        <strong>SMS Message</strong>
                    </td>
                    <td align=""left"">
                        <textarea id=""txtnopopup"" maxlength=""160"" class=""smstextarea"" rows=""4"" cols=""3""></textarea>
   ");
                WriteLiteral(@"                 </td>
                </tr>
                <tr id=""charlength"">
                    <td align=""left"" class=""auto-style3"">
                        &nbsp;
                    </td>
                    <td align=""left"">
                        <input id=""txtcountremcharacters"" value=""160"" readonly type=""text"" class=""textmedium"" style=""width: 25px;"" />
                        <label id=""lblCharAvlble"">Characters Available</label>
                    </td>
                </tr>
            </table>
            <table style=""left: 200px; position: relative;"">
                <tr>
                    <td>
                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "73f66ad3941720da35b15d2b7d47ec2c0574d52517794", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                    </td>
                    <td>
                        <input type=""hidden"" id=""hdnsubmit"" />
                    </td>
                </tr>
            </table>
            <div>
                <label id=""msg-response"" class=""error-Label""></label>
            </div>
            <table style=""width:100%;"" cellspacing=""20"" cellpadding=""0"" border=""0"" align=""center"">
                <tr>
                    <td align=""left"" colspan=""1"" class=""auto-style3"">
                        <label id=""lblTemplate"" class=""label"" hidden>Template</label><br />
                    </td>
                    <td align=""left"">
                        <select id=""drdptemplate"" class=""dropdown"" style=""width:250px;"" hidden");
                BeginWriteAttribute("onchange", " onchange=\"", 6036, "\"", 6047, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "73f66ad3941720da35b15d2b7d47ec2c0574d52520117", async() => {
                    WriteLiteral("---Select Template---");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_8.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                        </select><br />
                    </td>
                </tr>
                <tr>
                    <td align=""left"" width=""150px"">
                        <label id=""lbltypeofcomn"" class=""label"" hidden>Type of Communication</label>
                        <label id=""lbltypestar"" style=""color:red;"" hidden>*</label><br />
                    </td>
                    <td align=""left"">
                        <span id=""radcommunication"" hidden>
                            <label><input type=""radio"" name=""radcommunication"" value=""True"" style=""vertical-align:middle; margin-right:5px;"" />Internal</label>
                            <label>
                                <input type=""radio"" name=""radcommunication"" value=""False""
                                       style=""vertical-align:middle; margin-right:5px; margin-left:5px;"" />Customer Facing
                            </label>
                        </span>
                        <br />
                    ");
                WriteLiteral(@"</td>
                </tr>
                <tr>
                    <td>
                        <input type=""hidden"" id=""hdnname"" />
                        <input type=""hidden"" id=""hdndate"" />
                        <input type=""hidden"" id=""hdntime"" />
                        <input type=""hidden"" id=""hdnphnumber"" />
                        <input type=""hidden"" id=""hdnsmsmessage"" />
                        <input type=""hidden"" id=""hdnmessage"" />
                        <input type=""hidden"" id=""hdnrefno"" />
                        <input type=""hidden"" id=""hdnreason"" />
                        <input type=""hidden"" id=""hdnkellytime"" />
                        <input type=""hidden"" id=""hdnfromkelly"" />
                        <input type=""hidden"" id=""hdnextn"" />
                        <input type=""hidden"" id=""hdneagentname"" />
                        <input type=""hidden"" id=""hdnstoretxt"" />
                        <input type=""hidden"" id=""hdnaddress1text"" />
                        <input typ");
                WriteLiteral("e=\"hidden\" id=\"hdnaddress2text\" />\r\n                        <input type=\"hidden\" id=\"hdnpostcode\" />\r\n                    </td>\r\n                </tr>\r\n            </table>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
    </div>

    <div id=""popupdiv"" class=""popupclass"">
        <table>
            <tr id=""lblMsg"">
                <td colspan=""2"">
                    <textarea id=""Txtmsglabel"" readonly style=""width:300px;""></textarea>
                </td>
            </tr>
            <tr id=""refno"">
                <td class=""style1"">
                    Reference Number
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text"" id=""Txtpopuprefno"" maxlength=""8"" class=""onlynumberics"" />
                </td>
            </tr>

            <tr id=""tragentname"" >
                <td class=""style1"">
                    Agent Name
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text"" id=""txtagentname"" maxlength=""15"" class=""onlyalphabets"" />
                </td>
            </tr>

            <tr id=""Trextn"">
                <td class=""style1"">
                    Extention Number
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text"" id=""txtextn""  maxlength=""4"" class=""onlynumberics""/>
                </td>
            </tr>

            <tr id=""name"">
                <td class=""style1"">
                    Name
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text"" id=""Txtpopupname"" maxlength=""11"" class=""onlyalphabets""/>
                </td>
            </tr>
            <tr id=""hidetxtlabl"">
                <td class=""style1"">
                </td>
                <td >
");
            WriteLiteral(@"
                    <input type=""text"" id=""Txtnamecount"" name=""chars_available"" value=""11"" class=""textmedium"" readonly style=""width:16px;"" />
                    <label id=""lblnamecount"" style=""font-size:smaller;"">Characters Available</label>
                </td>
            </tr>
            <tr id=""date"">
                <td class=""style1"">
                    Date
                </td>

                <td>
");
            WriteLiteral("                    <input type=\"text\" id=\"Txtpopupdate\"/>\r\n                </td>\r\n            </tr>\r\n\r\n            <tr id=\"time\">\r\n                <td class=\"style1\">\r\n                    Time\r\n                </td>\r\n                <td>\r\n");
            WriteLiteral(@"
                    <input type=""text""  id=""Ddlpopuptime"" value=""AM-PM"" maxlength=""9""/>
                    <label id=""Lbltimeformat"" style=""color: red; font-size: xx-small; display: block;"">Eg: 8AM-12PM</label>
                </td>
            </tr>

            <tr id=""Kellypopuptime"">
                <td class=""style1"">
                    Time
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text"" id=""Ddlpopupkellytime"" value=""AM-PM"" maxlength=""9"" />
                    <label id=""lblkellytimeformat"" style=""color:red;font-size:xx-small;display:block;"">Eg: 8AM-12PM</label>
                </td>
            </tr>
            <tr id=""number"">
                <td class=""style1"">
                    Contact Number
                </td>
                <td>
");
            WriteLiteral(@"
                    <input type=""text""  id=""Txtpopupnumber"" maxlength=""12"" class=""onlynumberics""/>
                </td>
            </tr>
            <tr id=""reason"">
                <td class=""style1"">
                    Reason
                </td>
            </tr>
            <tr>
                <td>
");
            WriteLiteral(@"
                    <textarea id=""Txtpopupreason"" maxlength=""32"" rows=""4"" cols=""20"" name=""S1""
                              class=""smstextarea"" style=""border-style: solid; border-color: #000000; white-space: normal;""> </textarea>
                </td>
            </tr>

            <tr id=""reasoncount"">
                <td align=""right"">
");
            WriteLiteral(@"                    <input type=""text"" id=""Txtreasoncount"" name=""chars_available"" value=""32"" class=""textmedium"" readonly style=""width:20px;""/>
                    <label id=""lblreasoncount"" class=""para"">Characters Available</label>
                    <br />
                    <br />
                </td>
            </tr>

");
            WriteLiteral("\r\n            <tr id=\"StoreNamelbl\">\r\n                <td class=\"style1\">\r\n                    Store Name\r\n                </td>\r\n                <td colspan=\"2\">\r\n");
            WriteLiteral("                    <input type=\"text\" id=\"StoreNameText\" maxlength=\"30\" style=\"width:95%; height:30%;\" />\r\n                </td>\r\n            </tr>\r\n\r\n");
            WriteLiteral("\r\n            <tr id=\"Address1lbl\">\r\n                <td class=\"style1\">\r\n                    Address 1\r\n                </td>\r\n                <td colspan=\"2\">\r\n");
            WriteLiteral("\r\n                    <textarea id=\"Address1Text\" maxlength=\"130\" style=\"width:95%;\" disabled></textarea>\r\n                </td>\r\n            </tr>\r\n\r\n");
            WriteLiteral("\r\n            <tr id=\"Address2lbl\" visible=\"false\" runat=\"server\">\r\n                <td class=\"style1\">\r\n                    Address 2\r\n                </td>\r\n                <td colspan=\"2\">\r\n");
            WriteLiteral("\r\n                    <textarea id=\"Address2Text\" maxlength=\"130\" style=\"width:95%;\" disabled></textarea>\r\n                </td>\r\n            </tr>\r\n\r\n");
            WriteLiteral("\r\n            <tr id=\"PostCodelbl\">\r\n                <td class=\"style1\">\r\n                    Post Code\r\n                </td>\r\n                <td>\r\n");
            WriteLiteral("\r\n                    <input type=\"text\" id=\"PostCodeText\" maxlength=\"10\" n />\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SMSControlPanel.Models.UserDetails> Html { get; private set; }
    }
}
#pragma warning restore 1591
