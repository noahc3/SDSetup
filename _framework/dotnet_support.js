// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var DotNetSupportLib = {
	$DOTNET: {
		conv_string: function (mono_obj) {
			return MONO.string_decoder.copy (mono_obj);
		}
	},
	mono_wasm_invoke_js_blazor: function(exceptionMessage, callInfo, arg0, arg1, arg2)	{
		var mono_string = globalThis._mono_string_cached
			|| (globalThis._mono_string_cached = Module.cwrap('mono_wasm_string_from_js', 'number', ['string']));

		try {
			var blazorExports = globalThis.Blazor;
			if (!blazorExports) {
				throw new Error('The blazor.webassembly.js library is not loaded.');
			}

			return blazorExports._internal.invokeJSFromDotNet(callInfo, arg0, arg1, arg2);
		} catch (ex) {
			var exceptionJsString = ex.message + '\n' + ex.stack;
			var exceptionSystemString = mono_string(exceptionJsString);
			setValue (exceptionMessage, exceptionSystemString, 'i32'); // *exceptionMessage = exceptionSystemString;
			return 0;
		}
	},

	// This is for back-compat only and will eventually be removed
	mono_wasm_invoke_js_marshalled: function(exceptionMessage, asyncHandleLongPtr, functionName, argsJson, treatResultAsVoid) {

		var mono_string = globalThis._mono_string_cached
			|| (globalThis._mono_string_cached = Module.cwrap('mono_wasm_string_from_js', 'number', ['string']));

		try {
			// Passing a .NET long into JS via Emscripten is tricky. The method here is to pass
			// as pointer to the long, then combine two reads from the HEAPU32 array.
			// Even though JS numbers can't represent the full range of a .NET long, it's OK
			// because we'll never exceed Number.MAX_SAFE_INTEGER (2^53 - 1) in this case.
			//var u32Index = $1 >> 2;
			var u32Index = asyncHandleLongPtr >> 2;
			var asyncHandleJsNumber = Module.HEAPU32[u32Index + 1]*4294967296 + Module.HEAPU32[u32Index];

			// var funcNameJsString = UTF8ToString (functionName);
			// var argsJsonJsString = argsJson && UTF8ToString (argsJson);
			var funcNameJsString = DOTNET.conv_string(functionName);
			var argsJsonJsString = argsJson && DOTNET.conv_string (argsJson);

			var dotNetExports = globaThis.DotNet;
			if (!dotNetExports) {
				throw new Error('The Microsoft.JSInterop.js library is not loaded.');
			}

			if (asyncHandleJsNumber) {
				dotNetExports.jsCallDispatcher.beginInvokeJSFromDotNet(asyncHandleJsNumber, funcNameJsString, argsJsonJsString, treatResultAsVoid);
				return 0;
			} else {
				var resultJson = dotNetExports.jsCallDispatcher.invokeJSFromDotNet(funcNameJsString, argsJsonJsString, treatResultAsVoid);
				return resultJson === null ? 0 : mono_string(resultJson);
			}
		} catch (ex) {
			var exceptionJsString = ex.message + '\n' + ex.stack;
			var exceptionSystemString = mono_string(exceptionJsString);
			setValue (exceptionMessage, exceptionSystemString, 'i32'); // *exceptionMessage = exceptionSystemString;
			return 0;
		}
	},

	// This is for back-compat only and will eventually be removed
	mono_wasm_invoke_js_unmarshalled: function(exceptionMessage, funcName, arg0, arg1, arg2)	{
		try {
			// Get the function you're trying to invoke
			var funcNameJsString = DOTNET.conv_string(funcName);
			var dotNetExports = globalThis.DotNet;
			if (!dotNetExports) {
				throw new Error('The Microsoft.JSInterop.js library is not loaded.');
			}
			var funcInstance = dotNetExports.jsCallDispatcher.findJSFunction(funcNameJsString);

			return funcInstance.call(null, arg0, arg1, arg2);
		} catch (ex) {
			var exceptionJsString = ex.message + '\n' + ex.stack;
			var mono_string = Module.cwrap('mono_wasm_string_from_js', 'number', ['string']); // TODO: Cache
			var exceptionSystemString = mono_string(exceptionJsString);
			setValue (exceptionMessage, exceptionSystemString, 'i32'); // *exceptionMessage = exceptionSystemString;
			return 0;
		}
	}


};

autoAddDeps(DotNetSupportLib, '$DOTNET')
mergeInto(LibraryManager.library, DotNetSupportLib)


// SIG // Begin signature block
// SIG // MIIkfgYJKoZIhvcNAQcCoIIkbzCCJGsCAQExDzANBglg
// SIG // hkgBZQMEAgEFADB3BgorBgEEAYI3AgEEoGkwZzAyBgor
// SIG // BgEEAYI3AgEeMCQCAQEEEBDgyQbOONQRoqMAEEvTUJAC
// SIG // AQACAQACAQACAQACAQAwMTANBglghkgBZQMEAgEFAAQg
// SIG // DQxM5aUosMIFmI353dLRcro0jWTAvinWkY5FLkoNZqCg
// SIG // gg3wMIIGbjCCBFagAwIBAgITMwAAAhOMDBwxNbzSXwAA
// SIG // AAACEzANBgkqhkiG9w0BAQwFADB+MQswCQYDVQQGEwJV
// SIG // UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
// SIG // UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
// SIG // cmF0aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBT
// SIG // aWduaW5nIFBDQSAyMDExMB4XDTIxMDIxMTIwMDk1MVoX
// SIG // DTIyMDIxMDIwMDk1MVowYzELMAkGA1UEBhMCVVMxEzAR
// SIG // BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1v
// SIG // bmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
// SIG // bjENMAsGA1UEAxMELk5FVDCCAaIwDQYJKoZIhvcNAQEB
// SIG // BQADggGPADCCAYoCggGBAJtZcELdrGHlHCF6nz4bH8vW
// SIG // l5M3GfXIf7JY7OovRwgweTptJQGby0YHZ+iCrWIE7fTc
// SIG // /c9eGKGm+EsuWHnanAm9Ro7MSjdPsYBRaif1Y6dyhBcb
// SIG // b44guUNKlplq7L1k3ldXFFzyAt+u8UzCL5QFwibg2nWi
// SIG // QmCkoJWhiA6RxEPgEZ/ss2ICppgLHm1o6vy1P4ci6aMk
// SIG // Tj2s1uct/oFflYwE0DsK1OrFH7QvoIqWCAuXUXjZOKnF
// SIG // oRia22+ci2oxs/LVkgqcMwC35KHvUBzCW3LME/dSBWCO
// SIG // TV7gieG+gUtxBgPpzomak4thtrQLMRAWl7AOtI7QvsXa
// SIG // FEyQpAlDVz12Sa89KJOLBPksBRDw4woRZLlHnUrtxFRp
// SIG // MZsr+9cf2zfZPG4ia2iDSBFfXu2BeXrifkT4c/UV5Iy3
// SIG // qEHCzh1jLmN701jUOhF1QN1LEPn+TCth2b239/34+Bym
// SIG // cIAcDP1EWk8JodsUDedKhK+lAefNL0mzUrIQc6Dxb5cq
// SIG // may/QQIDAQABo4IBfjCCAXowHwYDVR0lBBgwFgYKKwYB
// SIG // BAGCN0wIAQYIKwYBBQUHAwMwHQYDVR0OBBYEFO9NaSC3
// SIG // 3IwsQ0OKpWHnclste605MFAGA1UdEQRJMEekRTBDMSkw
// SIG // JwYDVQQLEyBNaWNyb3NvZnQgT3BlcmF0aW9ucyBQdWVy
// SIG // dG8gUmljbzEWMBQGA1UEBRMNNDY0MjIzKzQ2NDI5MzAf
// SIG // BgNVHSMEGDAWgBRIbmTlUAXTgqoXNzcitW2oynUClTBU
// SIG // BgNVHR8ETTBLMEmgR6BFhkNodHRwOi8vd3d3Lm1pY3Jv
// SIG // c29mdC5jb20vcGtpb3BzL2NybC9NaWNDb2RTaWdQQ0Ey
// SIG // MDExXzIwMTEtMDctMDguY3JsMGEGCCsGAQUFBwEBBFUw
// SIG // UzBRBggrBgEFBQcwAoZFaHR0cDovL3d3dy5taWNyb3Nv
// SIG // ZnQuY29tL3BraW9wcy9jZXJ0cy9NaWNDb2RTaWdQQ0Ey
// SIG // MDExXzIwMTEtMDctMDguY3J0MAwGA1UdEwEB/wQCMAAw
// SIG // DQYJKoZIhvcNAQEMBQADggIBAFiD+cR0K6evMUeUrBMA
// SIG // pLljV65GDDTzlD4jqr6Mu1NTeZv5L9IJlR6DLAEKaJnB
// SIG // a7fZZ/ME/FZasmc40+WijhDmth/OOc7IpfJ3Ra1auKIA
// SIG // g687mo/eWiPs0nC42oCdchy9Q5AS7K0+MUk7R/p9eCTP
// SIG // NYFjSMItiL+YFYCxaZXqHizwdXcvCIrESq4DXwN+ZdUe
// SIG // GBEO9F2SkMVC61/y2xwSwRWmfO/l4YutKT+dSKjlelYi
// SIG // zFAQaJrGzO5ac56S+K/NMndPL7Od3ohqxMu7gsFUynxY
// SIG // l+eyB9T9I9HrUWoHj6ce4nzOxHC+yDRD6Mi2AaT+IbMO
// SIG // cGvWeJC5iX3tzpMqdz0BOMl6jbff+t+BLS7VtU6JAFCM
// SIG // fk5h+wqIPWjon3tpTuFtCkMOSzIoso3U6kdX0fgrgXnN
// SIG // KJspBXkfKG9lMPOPOKwzua1qjghvgzPMftj1yZqFljJm
// SIG // cjBxs/HKA8J8st1MKcgiBGDX5zkcsHYGuAkIb2fXQuYW
// SIG // y0G78JzzSv1u0LAFj8/Qtx9Hm2wfc20+ww+MYEQ9tu1F
// SIG // uJZK2O7+p7iVziwZvo+XVzuIU7sVjcmJH5Gn/ttfkLQ3
// SIG // 0jvM9QyV/lYwurg4Gn5Li/IZSN56WGIPilRkXUVurpaV
// SIG // WeYCjeUJzMY2n2tVMFl6pgnGmaA2a0uiG3z0GpMPdbS1
// SIG // R/oEMIIHejCCBWKgAwIBAgIKYQ6Q0gAAAAAAAzANBgkq
// SIG // hkiG9w0BAQsFADCBiDELMAkGA1UEBhMCVVMxEzARBgNV
// SIG // BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
// SIG // HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEy
// SIG // MDAGA1UEAxMpTWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNh
// SIG // dGUgQXV0aG9yaXR5IDIwMTEwHhcNMTEwNzA4MjA1OTA5
// SIG // WhcNMjYwNzA4MjEwOTA5WjB+MQswCQYDVQQGEwJVUzET
// SIG // MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVk
// SIG // bW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0
// SIG // aW9uMSgwJgYDVQQDEx9NaWNyb3NvZnQgQ29kZSBTaWdu
// SIG // aW5nIFBDQSAyMDExMIICIjANBgkqhkiG9w0BAQEFAAOC
// SIG // Ag8AMIICCgKCAgEAq/D6chAcLq3YbqqCEE00uvK2WCGf
// SIG // Qhsqa+laUKq4BjgaBEm6f8MMHt03a8YS2AvwOMKZBrDI
// SIG // OdUBFDFC04kNeWSHfpRgJGyvnkmc6Whe0t+bU7IKLMOv
// SIG // 2akrrnoJr9eWWcpgGgXpZnboMlImEi/nqwhQz7NEt13Y
// SIG // xC4Ddato88tt8zpcoRb0RrrgOGSsbmQ1eKagYw8t00CT
// SIG // +OPeBw3VXHmlSSnnDb6gE3e+lD3v++MrWhAfTVYoonpy
// SIG // 4BI6t0le2O3tQ5GD2Xuye4Yb2T6xjF3oiU+EGvKhL1nk
// SIG // kDstrjNYxbc+/jLTswM9sbKvkjh+0p2ALPVOVpEhNSXD
// SIG // OW5kf1O6nA+tGSOEy/S6A4aN91/w0FK/jJSHvMAhdCVf
// SIG // GCi2zCcoOCWYOUo2z3yxkq4cI6epZuxhH2rhKEmdX4ji
// SIG // JV3TIUs+UsS1Vz8kA/DRelsv1SPjcF0PUUZ3s/gA4bys
// SIG // AoJf28AVs70b1FVL5zmhD+kjSbwYuER8ReTBw3J64HLn
// SIG // JN+/RpnF78IcV9uDjexNSTCnq47f7Fufr/zdsGbiwZeB
// SIG // e+3W7UvnSSmnEyimp31ngOaKYnhfsi+E11ecXL93KCjx
// SIG // 7W3DKI8sj0A3T8HhhUSJxAlMxdSlQy90lfdu+HggWCwT
// SIG // XWCVmj5PM4TasIgX3p5O9JawvEagbJjS4NaIjAsCAwEA
// SIG // AaOCAe0wggHpMBAGCSsGAQQBgjcVAQQDAgEAMB0GA1Ud
// SIG // DgQWBBRIbmTlUAXTgqoXNzcitW2oynUClTAZBgkrBgEE
// SIG // AYI3FAIEDB4KAFMAdQBiAEMAQTALBgNVHQ8EBAMCAYYw
// SIG // DwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBRyLToC
// SIG // MZBDuRQFTuHqp8cx0SOJNDBaBgNVHR8EUzBRME+gTaBL
// SIG // hklodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpL2Ny
// SIG // bC9wcm9kdWN0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFf
// SIG // MDNfMjIuY3JsMF4GCCsGAQUFBwEBBFIwUDBOBggrBgEF
// SIG // BQcwAoZCaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3Br
// SIG // aS9jZXJ0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFfMDNf
// SIG // MjIuY3J0MIGfBgNVHSAEgZcwgZQwgZEGCSsGAQQBgjcu
// SIG // AzCBgzA/BggrBgEFBQcCARYzaHR0cDovL3d3dy5taWNy
// SIG // b3NvZnQuY29tL3BraW9wcy9kb2NzL3ByaW1hcnljcHMu
// SIG // aHRtMEAGCCsGAQUFBwICMDQeMiAdAEwAZQBnAGEAbABf
// SIG // AHAAbwBsAGkAYwB5AF8AcwB0AGEAdABlAG0AZQBuAHQA
// SIG // LiAdMA0GCSqGSIb3DQEBCwUAA4ICAQBn8oalmOBUeRou
// SIG // 09h0ZyKbC5YR4WOSmUKWfdJ5DJDBZV8uLD74w3LRbYP+
// SIG // vj/oCso7v0epo/Np22O/IjWll11lhJB9i0ZQVdgMknzS
// SIG // Gksc8zxCi1LQsP1r4z4HLimb5j0bpdS1HXeUOeLpZMlE
// SIG // PXh6I/MTfaaQdION9MsmAkYqwooQu6SpBQyb7Wj6aC6V
// SIG // oCo/KmtYSWMfCWluWpiW5IP0wI/zRive/DvQvTXvbiWu
// SIG // 5a8n7dDd8w6vmSiXmE0OPQvyCInWH8MyGOLwxS3OW560
// SIG // STkKxgrCxq2u5bLZ2xWIUUVYODJxJxp/sfQn+N4sOiBp
// SIG // mLJZiWhub6e3dMNABQamASooPoI/E01mC8CzTfXhj38c
// SIG // bxV9Rad25UAqZaPDXVJihsMdYzaXht/a8/jyFqGaJ+HN
// SIG // pZfQ7l1jQeNbB5yHPgZ3BtEGsXUfFL5hYbXw3MYbBL7f
// SIG // QccOKO7eZS/sl/ahXJbYANahRr1Z85elCUtIEJmAH9AA
// SIG // KcWxm6U/RXceNcbSoqKfenoi+kiVH6v7RyOA9Z74v2u3
// SIG // S5fi63V4GuzqN5l5GEv/1rMjaHXmr/r8i+sLgOppO6/8
// SIG // MO0ETI7f33VtY5E90Z1WTk+/gFcioXgRMiF670EKsT/7
// SIG // qMykXcGhiJtXcVZOSEXAQsmbdlsKgEhr/Xmfwb1tbWrJ
// SIG // UnMTDXpQzTGCFeYwghXiAgEBMIGVMH4xCzAJBgNVBAYT
// SIG // AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
// SIG // EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
// SIG // cG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2Rl
// SIG // IFNpZ25pbmcgUENBIDIwMTECEzMAAAITjAwcMTW80l8A
// SIG // AAAAAhMwDQYJYIZIAWUDBAIBBQCgga4wGQYJKoZIhvcN
// SIG // AQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGCNwIBCzEO
// SIG // MAwGCisGAQQBgjcCARUwLwYJKoZIhvcNAQkEMSIEIPei
// SIG // aMpT7aW0G0eaO4V7u+OYsUIWBD2V6EuD1ajffj24MEIG
// SIG // CisGAQQBgjcCAQwxNDAyoBSAEgBNAGkAYwByAG8AcwBv
// SIG // AGYAdKEagBhodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20w
// SIG // DQYJKoZIhvcNAQEBBQAEggGASEblTgnTmkn+iGPTMgPr
// SIG // +5uptto3kTMfvkelPQ3w/xHmtXcAdH24QMN1aSgRgQkH
// SIG // oHsEzQgzSVBwR5jRH/yeM8rA3JqNHY7Gqo1Mhjz6KSxh
// SIG // 4n1YHmD+QYcpt1jSkqWvV6APQhsYyFv+0SsiAErlDRrv
// SIG // vuIHM5xjmVi5yjW2GGOLDtKN0UOtWS3Fagjdv4L02kXi
// SIG // a4XX9rRjzh8ykJawJrd/OgFKbBZlk/O/5ClWVMQ0ijoQ
// SIG // 5Gb2olho4CZ3M0cqsCigz8GMtQDEKb2y4MYYUrKHlkmo
// SIG // tW+q5Jqhw+ocM7sOIz/EIWfjw/D+Lz44N2oQag0MwW0o
// SIG // 0JBk4TcqIHkiYZ1At70J118HmWllMsh35TDgZTD6Ds7W
// SIG // NEG9Y7e+0GwRiva/wLQq/L6vGDtMfBXBJCwpNuwlniok
// SIG // bY4LFW45OtTkCBleFPyuTyNzz7DkG0+XJhE5fw3H6Tsy
// SIG // VgHbV3fAUM76vS/rVrihi6M+M7HejkC7il15R8lqm2La
// SIG // NUG0IugSoYIS8DCCEuwGCisGAQQBgjcDAwExghLcMIIS
// SIG // 2AYJKoZIhvcNAQcCoIISyTCCEsUCAQMxDzANBglghkgB
// SIG // ZQMEAgEFADCCAVQGCyqGSIb3DQEJEAEEoIIBQwSCAT8w
// SIG // ggE7AgEBBgorBgEEAYRZCgMBMDEwDQYJYIZIAWUDBAIB
// SIG // BQAEILs+o5Vp2Uu3D+piTqZS72r4qxA46uSkXA4muDH6
// SIG // FxXaAgZg05B3JDkYEjIwMjEwNzA2MDI0MzExLjc3WjAE
// SIG // gAIB9KCB1KSB0TCBzjELMAkGA1UEBhMCVVMxEzARBgNV
// SIG // BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
// SIG // HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEp
// SIG // MCcGA1UECxMgTWljcm9zb2Z0IE9wZXJhdGlvbnMgUHVl
// SIG // cnRvIFJpY28xJjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNO
// SIG // OkY3N0YtRTM1Ni01QkFFMSUwIwYDVQQDExxNaWNyb3Nv
// SIG // ZnQgVGltZS1TdGFtcCBTZXJ2aWNloIIORDCCBPUwggPd
// SIG // oAMCAQICEzMAAAFenSnHX4cFoeoAAAAAAV4wDQYJKoZI
// SIG // hvcNAQELBQAwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgT
// SIG // Cldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
// SIG // BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQG
// SIG // A1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIw
// SIG // MTAwHhcNMjEwMTE0MTkwMjE5WhcNMjIwNDExMTkwMjE5
// SIG // WjCBzjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hp
// SIG // bmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoT
// SIG // FU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEpMCcGA1UECxMg
// SIG // TWljcm9zb2Z0IE9wZXJhdGlvbnMgUHVlcnRvIFJpY28x
// SIG // JjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNOOkY3N0YtRTM1
// SIG // Ni01QkFFMSUwIwYDVQQDExxNaWNyb3NvZnQgVGltZS1T
// SIG // dGFtcCBTZXJ2aWNlMIIBIjANBgkqhkiG9w0BAQEFAAOC
// SIG // AQ8AMIIBCgKCAQEAmtMjg5B6GfegqnbO6HpY/ZmJv8PH
// SIG // D+yst57JNv153s9f58uDvMEDTKXqK8XafqVq4YfxbsQH
// SIG // BE8S/tkJJfBeBhnoYZofxpT46sNcBtzgFdM7lecsbBJt
// SIG // rJ71Hb65Ad0ImZoy3P+UQFZQrnG8eiPRNStc5l1n++/t
// SIG // OoxYDiHUBPXD8kFHiQe1XWLwpZ2VD51lf+A0ekDvYigu
// SIG // g6akiZsZHNwZDhnYrOrh4wH3CNoVFXUkX/DPWEsUiMa2
// SIG // VTd4aNEGIEQRUjtQQwxK8jisr4J8sAhmdQu7tLOUh+pJ
// SIG // TdHSlI1RqHClZ0KIHp8rMir3hn73zzyahC6j3lEA+bMd
// SIG // BbUwjQIDAQABo4IBGzCCARcwHQYDVR0OBBYEFKpyfLoN
// SIG // 3UvlVMIQAJ7OVHjV+B8rMB8GA1UdIwQYMBaAFNVjOlyK
// SIG // MZDzQ3t8RhvFM2hahW1VMFYGA1UdHwRPME0wS6BJoEeG
// SIG // RWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3Js
// SIG // L3Byb2R1Y3RzL01pY1RpbVN0YVBDQV8yMDEwLTA3LTAx
// SIG // LmNybDBaBggrBgEFBQcBAQROMEwwSgYIKwYBBQUHMAKG
// SIG // Pmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kvY2Vy
// SIG // dHMvTWljVGltU3RhUENBXzIwMTAtMDctMDEuY3J0MAwG
// SIG // A1UdEwEB/wQCMAAwEwYDVR0lBAwwCgYIKwYBBQUHAwgw
// SIG // DQYJKoZIhvcNAQELBQADggEBAH8h/FmExiQEypGZeeH9
// SIG // WK3ht/HKKgCWvscnVcNIdMi9HAMPU8avS6lkT++usj9A
// SIG // 3/VaLq8NwqacnavtePqlZk5mpz0Gn64G+k9q6W57iy27
// SIG // dOopNz0W7YrmJty2kXigc99n4gp4KGin4yT2Ds3mWUfO
// SIG // /RoIOJozTDZoBPeuPdAdBLyKOdDn+qG3PCjUChSdXXLa
// SIG // 6tbBflod1TNqh4Amu+d/Z57z0p/jJyOPJp80lJSn+ppc
// SIG // GVuMy73S825smy11LE62/BzF54c/plphtOXZw6VrhyiS
// SIG // I9T4FSMhkD+38hl9ihrMwaYG0tYUll0L0thZaYsuw0nZ
// SIG // bbWqR5JKkQDDimYwggZxMIIEWaADAgECAgphCYEqAAAA
// SIG // AAACMA0GCSqGSIb3DQEBCwUAMIGIMQswCQYDVQQGEwJV
// SIG // UzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
// SIG // UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
// SIG // cmF0aW9uMTIwMAYDVQQDEylNaWNyb3NvZnQgUm9vdCBD
// SIG // ZXJ0aWZpY2F0ZSBBdXRob3JpdHkgMjAxMDAeFw0xMDA3
// SIG // MDEyMTM2NTVaFw0yNTA3MDEyMTQ2NTVaMHwxCzAJBgNV
// SIG // BAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYD
// SIG // VQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
// SIG // Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
// SIG // aW1lLVN0YW1wIFBDQSAyMDEwMIIBIjANBgkqhkiG9w0B
// SIG // AQEFAAOCAQ8AMIIBCgKCAQEAqR0NvHcRijog7PwTl/X6
// SIG // f2mUa3RUENWlCgCChfvtfGhLLF/Fw+Vhwna3PmYrW/AV
// SIG // UycEMR9BGxqVHc4JE458YTBZsTBED/FgiIRUQwzXTbg4
// SIG // CLNC3ZOs1nMwVyaCo0UN0Or1R4HNvyRgMlhgRvJYR4Yy
// SIG // hB50YWeRX4FUsc+TTJLBxKZd0WETbijGGvmGgLvfYfxG
// SIG // wScdJGcSchohiq9LZIlQYrFd/XcfPfBXday9ikJNQFHR
// SIG // D5wGPmd/9WbAA5ZEfu/QS/1u5ZrKsajyeioKMfDaTgaR
// SIG // togINeh4HLDpmc085y9Euqf03GS9pAHBIAmTeM38vMDJ
// SIG // RF1eFpwBBU8iTQIDAQABo4IB5jCCAeIwEAYJKwYBBAGC
// SIG // NxUBBAMCAQAwHQYDVR0OBBYEFNVjOlyKMZDzQ3t8RhvF
// SIG // M2hahW1VMBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBB
// SIG // MAsGA1UdDwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8G
// SIG // A1UdIwQYMBaAFNX2VsuP6KJcYmjRPZSQW9fOmhjEMFYG
// SIG // A1UdHwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwubWljcm9z
// SIG // b2Z0LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0Nl
// SIG // ckF1dF8yMDEwLTA2LTIzLmNybDBaBggrBgEFBQcBAQRO
// SIG // MEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93d3cubWljcm9z
// SIG // b2Z0LmNvbS9wa2kvY2VydHMvTWljUm9vQ2VyQXV0XzIw
// SIG // MTAtMDYtMjMuY3J0MIGgBgNVHSABAf8EgZUwgZIwgY8G
// SIG // CSsGAQQBgjcuAzCBgTA9BggrBgEFBQcCARYxaHR0cDov
// SIG // L3d3dy5taWNyb3NvZnQuY29tL1BLSS9kb2NzL0NQUy9k
// SIG // ZWZhdWx0Lmh0bTBABggrBgEFBQcCAjA0HjIgHQBMAGUA
// SIG // ZwBhAGwAXwBQAG8AbABpAGMAeQBfAFMAdABhAHQAZQBt
// SIG // AGUAbgB0AC4gHTANBgkqhkiG9w0BAQsFAAOCAgEAB+aI
// SIG // UQ3ixuCYP4FxAz2do6Ehb7Prpsz1Mb7PBeKp/vpXbRkw
// SIG // s8LFZslq3/Xn8Hi9x6ieJeP5vO1rVFcIK1GCRBL7uVOM
// SIG // zPRgEop2zEBAQZvcXBf/XPleFzWYJFZLdO9CEMivv3/G
// SIG // f/I3fVo/HPKZeUqRUgCvOA8X9S95gWXZqbVr5MfO9sp6
// SIG // AG9LMEQkIjzP7QOllo9ZKby2/QThcJ8ySif9Va8v/rbl
// SIG // jjO7Yl+a21dA6fHOmWaQjP9qYn/dxUoLkSbiOewZSnFj
// SIG // nXshbcOco6I8+n99lmqQeKZt0uGc+R38ONiU9MalCpaG
// SIG // pL2eGq4EQoO4tYCbIjggtSXlZOz39L9+Y1klD3ouOVd2
// SIG // onGqBooPiRa6YacRy5rYDkeagMXQzafQ732D8OE7cQnf
// SIG // XXSYIghh2rBQHm+98eEA3+cxB6STOvdlR3jo+KhIq/fe
// SIG // cn5ha293qYHLpwmsObvsxsvYgrRyzR30uIUBHoD7G4kq
// SIG // VDmyW9rIDVWZeodzOwjmmC3qjeAzLhIp9cAvVCch98is
// SIG // TtoouLGp25ayp0Kiyc8ZQU3ghvkqmqMRZjDTu3QyS99j
// SIG // e/WZii8bxyGvWbWu3EQ8l1Bx16HSxVXjad5XwdHeMMD9
// SIG // zOZN+w2/XU/pnR4ZOC+8z1gFLu8NoFA12u8JJxzVs341
// SIG // Hgi62jbb01+P3nSISRKhggLSMIICOwIBATCB/KGB1KSB
// SIG // 0TCBzjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hp
// SIG // bmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoT
// SIG // FU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEpMCcGA1UECxMg
// SIG // TWljcm9zb2Z0IE9wZXJhdGlvbnMgUHVlcnRvIFJpY28x
// SIG // JjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNOOkY3N0YtRTM1
// SIG // Ni01QkFFMSUwIwYDVQQDExxNaWNyb3NvZnQgVGltZS1T
// SIG // dGFtcCBTZXJ2aWNloiMKAQEwBwYFKw4DAhoDFQBWSY9X
// SIG // /yFlVL0XNu2hfbHdnbFjKqCBgzCBgKR+MHwxCzAJBgNV
// SIG // BAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYD
// SIG // VQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
// SIG // Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBU
// SIG // aW1lLVN0YW1wIFBDQSAyMDEwMA0GCSqGSIb3DQEBBQUA
// SIG // AgUA5I3gpjAiGA8yMDIxMDcwNTIzNDg1NFoYDzIwMjEw
// SIG // NzA2MjM0ODU0WjB3MD0GCisGAQQBhFkKBAExLzAtMAoC
// SIG // BQDkjeCmAgEAMAoCAQACAhzAAgH/MAcCAQACAhFBMAoC
// SIG // BQDkjzImAgEAMDYGCisGAQQBhFkKBAIxKDAmMAwGCisG
// SIG // AQQBhFkKAwKgCjAIAgEAAgMHoSChCjAIAgEAAgMBhqAw
// SIG // DQYJKoZIhvcNAQEFBQADgYEAoDdKOBPOYRcTVaINHkWk
// SIG // Mw83oBDFMUI2L2MiSdnuzL9QY7KUQx3yDA9gKWErwh2w
// SIG // QqoY/WGq5I113RBvffQ6CjwrAUBWGjOzFVsMmQRA8vgx
// SIG // wDuHHe4/SVgGmb68Xje7GlcKPPF+coQGyOg275pXuhCO
// SIG // vw9c1bjU9XG5zLzgI7AxggMNMIIDCQIBATCBkzB8MQsw
// SIG // CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQ
// SIG // MA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
// SIG // b2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3Nv
// SIG // ZnQgVGltZS1TdGFtcCBQQ0EgMjAxMAITMwAAAV6dKcdf
// SIG // hwWh6gAAAAABXjANBglghkgBZQMEAgEFAKCCAUowGgYJ
// SIG // KoZIhvcNAQkDMQ0GCyqGSIb3DQEJEAEEMC8GCSqGSIb3
// SIG // DQEJBDEiBCByENu07KNd7BqaX9ajiW4/SOWYAbxZ7k+g
// SIG // f/Nq9AEY2DCB+gYLKoZIhvcNAQkQAi8xgeowgecwgeQw
// SIG // gb0EIH7lhOyU1JeO4H7mZANMpGQzumuR7CFed69eku/x
// SIG // EtPiMIGYMIGApH4wfDELMAkGA1UEBhMCVVMxEzARBgNV
// SIG // BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
// SIG // HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEm
// SIG // MCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENB
// SIG // IDIwMTACEzMAAAFenSnHX4cFoeoAAAAAAV4wIgQg2QTe
// SIG // kJVADu5JeC2NjOxhrQD5gxt9x1y/SqjJBKQKqTMwDQYJ
// SIG // KoZIhvcNAQELBQAEggEAHOc9aPsnlb0fYxC1zYcmxvw9
// SIG // NAaeiCBkEAy99iD+CjdKg96Dpdhdy+UCGgbgIBPUZKrb
// SIG // fRsxUTl0R6+jWSifl4FPuYAhAhZqT8WT7AitP7RSfLzk
// SIG // G2N96oJYE1Xqjvxk6TN49eStQMSx0cxocE0cTQPxVfJn
// SIG // VOmVmAkkOdmFpfQ06WcDsyHmfnbMl1F9Ba4CqBGKxpI2
// SIG // /eXoiLdYY8Auqf8/UJqO3FGzq9+KMIfhtAX/1witmGmk
// SIG // dBhbNz/GuxTMX93couJZUczUhEdItq/bYn7X0u0Ap/hq
// SIG // JMX+GvzJHrO82OKG4CnjzOFN/OehyM3TNYNtJwR8kcks
// SIG // J+ktbYWkKw==
// SIG // End signature block
