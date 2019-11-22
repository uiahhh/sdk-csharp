(window.webpackJsonp=window.webpackJsonp||[]).push([[29],{278:function(e,t,a){"use strict";a.r(t);var s=a(0),r=Object(s.a)({},function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("ContentSlotsDistributor",{attrs:{"slot-key":e.$parent.slotKey}},[a("h1",{attrs:{id:"deletemycredentialsasync"}},[e._v("DeleteMyCredentialsAsync "),a("a",{staticClass:"header-anchor",attrs:{href:"#deletemycredentialsasync","aria-hidden":"true"}})]),e._v(" "),a("p",[e._v("Delete the current user's credentials for the specified strategy. If the credentials that generated the current JWT are removed, the user will remain logged in until he logs out or his session expires. After that they will no longer be able to log in with the deleted credentials.")]),e._v(" "),a("h2",{attrs:{id:"arguments"}},[e._v("Arguments "),a("a",{staticClass:"header-anchor",attrs:{href:"#arguments","aria-hidden":"true"}})]),e._v(" "),a("div",{staticClass:"code-block"},[a("div",{staticClass:"language-csharp extra-class"},[a("div",{staticClass:"codehilite",attrs:{id:"__code_9"}},[a("button",{staticClass:"md-clipboard",attrs:{title:"Copy to clipboard","data-clipboard-target":"#__code_9 pre code"}}),e._v(" "),a("span",{staticClass:"md-clipboard__message"},[e._v("Copied to clipboard!")])]),a("pre",{pre:!0,attrs:{class:"language-csharp"}},[a("code",[a("span",{pre:!0,attrs:{class:"token keyword"}},[e._v("public")]),e._v(" "),a("span",{pre:!0,attrs:{class:"token keyword"}},[e._v("async")]),e._v(" "),a("span",{pre:!0,attrs:{class:"token class-name"}},[e._v("Task")]),e._v(" "),a("span",{pre:!0,attrs:{class:"token function"}},[e._v("DeleteMyCredentialsAsync")]),a("span",{pre:!0,attrs:{class:"token punctuation"}},[e._v("(")]),a("span",{pre:!0,attrs:{class:"token keyword"}},[e._v("string")]),e._v(" strategy"),a("span",{pre:!0,attrs:{class:"token punctuation"}},[e._v(")")]),a("span",{pre:!0,attrs:{class:"token punctuation"}},[e._v(";")]),e._v("\n")])])])]),a("table",[a("thead",[a("tr",[a("th",[e._v("Argument")]),e._v(" "),a("th",[e._v("Type")]),e._v(" "),a("th",[e._v("Description")])])]),e._v(" "),a("tbody",[a("tr",[a("td",[a("code",[e._v("strategy")])]),e._v(" "),a("td",[a("pre",[e._v("string")])]),e._v(" "),a("td",[e._v("Strategy to use")])])])]),e._v(" "),a("h2",{attrs:{id:"exceptions"}},[e._v("Exceptions "),a("a",{staticClass:"header-anchor",attrs:{href:"#exceptions","aria-hidden":"true"}})]),e._v(" "),a("p",[e._v("Throws a "),a("code",[e._v("KuzzleException")]),e._v(" if there is an error. See how to "),a("a",{attrs:{href:"/sdk/csharp/1/essentials/error-handling"}},[e._v("handle error")]),e._v(".")]),e._v(" "),a("h2",{attrs:{id:"usage"}},[e._v("Usage "),a("a",{staticClass:"header-anchor",attrs:{href:"#usage","aria-hidden":"true"}})]),e._v(" "),a("div",{staticClass:"code-block"},[a("div",{staticClass:"language-cs extra-class"},[a("div",{staticClass:"codehilite",attrs:{id:"__code_47"}},[a("button",{staticClass:"md-clipboard",attrs:{title:"Copy to clipboard","data-clipboard-target":"#__code_47 pre code"}}),e._v(" "),a("span",{staticClass:"md-clipboard__message"},[e._v("Copied to clipboard!")])]),a("pre",{pre:!0,attrs:{class:"language-text"}},[a("code",[e._v('try {\n  await kuzzle.Auth.LoginAsync(\n    "local",\n    JObject.Parse("{username: \'foo\', password: \'bar\'}"));\n  await kuzzle.Auth.DeleteMyCredentialsAsync("local");\n\n  Console.WriteLine("Credentials Successfully deleted");\n} catch (KuzzleException e) {\n  Console.WriteLine(e);\n}\n')])])])])])},[],!1,null,null,null);t.default=r.exports}}]);