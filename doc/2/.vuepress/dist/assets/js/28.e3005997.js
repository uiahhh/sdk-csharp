(window.webpackJsonp=window.webpackJsonp||[]).push([[28],{277:function(t,a,s){"use strict";s.r(a);var e=s(0),r=Object(e.a)({},function(){var t=this,a=t.$createElement,s=t._self._c||a;return s("ContentSlotsDistributor",{attrs:{"slot-key":t.$parent.slotKey}},[s("h1",{attrs:{id:"credentialsexistasync"}},[t._v("CredentialsExistAsync "),s("a",{staticClass:"header-anchor",attrs:{href:"#credentialsexistasync","aria-hidden":"true"}})]),t._v(" "),s("p",[t._v("Check that the current user has credentials for the specified strategy.")]),t._v(" "),s("h2",{attrs:{id:"arguments"}},[t._v("Arguments "),s("a",{staticClass:"header-anchor",attrs:{href:"#arguments","aria-hidden":"true"}})]),t._v(" "),s("div",{staticClass:"code-block"},[s("div",{staticClass:"language-csharp extra-class"},[s("div",{staticClass:"codehilite",attrs:{id:"__code_9"}},[s("button",{staticClass:"md-clipboard",attrs:{title:"Copy to clipboard","data-clipboard-target":"#__code_9 pre code"}}),t._v(" "),s("span",{staticClass:"md-clipboard__message"},[t._v("Copied to clipboard!")])]),s("pre",{pre:!0,attrs:{class:"language-csharp"}},[s("code",[s("span",{pre:!0,attrs:{class:"token keyword"}},[t._v("public")]),t._v(" "),s("span",{pre:!0,attrs:{class:"token keyword"}},[t._v("async")]),t._v(" Task"),s("span",{pre:!0,attrs:{class:"token operator"}},[t._v("<")]),s("span",{pre:!0,attrs:{class:"token keyword"}},[t._v("bool")]),s("span",{pre:!0,attrs:{class:"token operator"}},[t._v(">")]),t._v(" "),s("span",{pre:!0,attrs:{class:"token function"}},[t._v("CredentialsExistAsync")]),s("span",{pre:!0,attrs:{class:"token punctuation"}},[t._v("(")]),s("span",{pre:!0,attrs:{class:"token keyword"}},[t._v("string")]),t._v(" strategy"),s("span",{pre:!0,attrs:{class:"token punctuation"}},[t._v(")")]),s("span",{pre:!0,attrs:{class:"token punctuation"}},[t._v(";")]),t._v("\n")])])])]),s("table",[s("thead",[s("tr",[s("th",[t._v("Argument")]),t._v(" "),s("th",[t._v("Type")]),t._v(" "),s("th",[t._v("Description")])])]),t._v(" "),s("tbody",[s("tr",[s("td",[s("code",[t._v("strategy")])]),t._v(" "),s("td",[s("pre",[t._v("string")])]),t._v(" "),s("td",[t._v("Strategy to use")])])])]),t._v(" "),s("h2",{attrs:{id:"return"}},[t._v("Return "),s("a",{staticClass:"header-anchor",attrs:{href:"#return","aria-hidden":"true"}})]),t._v(" "),s("p",[t._v("A boolean indicating if credentials exists for the strategy.")]),t._v(" "),s("h2",{attrs:{id:"exceptions"}},[t._v("Exceptions "),s("a",{staticClass:"header-anchor",attrs:{href:"#exceptions","aria-hidden":"true"}})]),t._v(" "),s("p",[t._v("Throws a "),s("code",[t._v("KuzzleException")]),t._v(" if there is an error. See how to "),s("a",{attrs:{href:"/sdk/csharp/1/essentials/error-handling"}},[t._v("handle error")]),t._v(".")]),t._v(" "),s("h2",{attrs:{id:"usage"}},[t._v("Usage "),s("a",{staticClass:"header-anchor",attrs:{href:"#usage","aria-hidden":"true"}})]),t._v(" "),s("div",{staticClass:"code-block"},[s("div",{staticClass:"language-cs extra-class"},[s("div",{staticClass:"codehilite",attrs:{id:"__code_53"}},[s("button",{staticClass:"md-clipboard",attrs:{title:"Copy to clipboard","data-clipboard-target":"#__code_53 pre code"}}),t._v(" "),s("span",{staticClass:"md-clipboard__message"},[t._v("Copied to clipboard!")])]),s("pre",{pre:!0,attrs:{class:"language-text"}},[s("code",[t._v('try {\n  await kuzzle.Auth.LoginAsync(\n    "local",\n    JObject.Parse("{username: \'foo\', password: \'bar\'}"));\n  bool exists = await kuzzle.Auth.CredentialsExistAsync("local");\n\n  if (exists) {\n    Console.WriteLine("Credentials exists for local strategy");\n  }\n} catch (KuzzleException e) {\n  Console.WriteLine(e);\n}\n')])])])])])},[],!1,null,null,null);a.default=r.exports}}]);