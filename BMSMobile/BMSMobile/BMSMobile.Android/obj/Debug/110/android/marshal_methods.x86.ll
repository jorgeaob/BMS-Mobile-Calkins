; ModuleID = 'obj\Debug\110\android\marshal_methods.x86.ll'
source_filename = "obj\Debug\110\android\marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [232 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 59
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 90
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 11
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 84
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 73
	i32 120558881, ; 5: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 73
	i32 165246403, ; 6: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 40
	i32 166922606, ; 7: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 24
	i32 172012715, ; 8: FastAndroidCamera.dll => 0xa40b4ab => 6
	i32 182336117, ; 9: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 74
	i32 202624640, ; 10: Xamarin.Forms.BehaviorsPack => 0xc13ce80 => 83
	i32 209399409, ; 11: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 38
	i32 219130465, ; 12: Xamarin.Android.Support.v4 => 0xd0faa61 => 29
	i32 220029658, ; 13: BMSMobile.Android.dll => 0xd1d62da => 0
	i32 220171995, ; 14: System.Diagnostics.Debug => 0xd1f8edb => 109
	i32 230216969, ; 15: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 54
	i32 231814094, ; 16: System.Globalization => 0xdd133ce => 115
	i32 232815796, ; 17: System.Web.Services => 0xde07cb4 => 103
	i32 261689757, ; 18: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 43
	i32 278686392, ; 19: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 58
	i32 280482487, ; 20: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 52
	i32 318968648, ; 21: Xamarin.AndroidX.Activity.dll => 0x13031348 => 30
	i32 321597661, ; 22: System.Numerics => 0x132b30dd => 18
	i32 334355562, ; 23: ZXing.Net.Mobile.Forms.dll => 0x13eddc6a => 93
	i32 342366114, ; 24: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 56
	i32 381494705, ; 25: Xamarin.Forms.Material => 0x16bd25b1 => 85
	i32 389971796, ; 26: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 25
	i32 441335492, ; 27: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 42
	i32 442521989, ; 28: Xamarin.Essentials => 0x1a605985 => 82
	i32 442565967, ; 29: System.Collections => 0x1a61054f => 107
	i32 450948140, ; 30: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 51
	i32 465846621, ; 31: mscorlib => 0x1bc4415d => 10
	i32 469710990, ; 32: System.dll => 0x1bff388e => 17
	i32 476646585, ; 33: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 52
	i32 486930444, ; 34: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 63
	i32 498788369, ; 35: System.ObjectModel => 0x1dbae811 => 110
	i32 514659665, ; 36: Xamarin.Android.Support.Compat => 0x1ead1551 => 24
	i32 526420162, ; 37: System.Transactions.dll => 0x1f6088c2 => 97
	i32 545304856, ; 38: System.Runtime.Extensions => 0x2080b118 => 108
	i32 605376203, ; 39: System.IO.Compression.FileSystem => 0x24154ecb => 101
	i32 627609679, ; 40: Xamarin.AndroidX.CustomView => 0x2568904f => 47
	i32 663517072, ; 41: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 79
	i32 666292255, ; 42: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 35
	i32 690569205, ; 43: System.Xml.Linq.dll => 0x29293ff5 => 22
	i32 692692150, ; 44: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 23
	i32 775507847, ; 45: System.IO.Compression => 0x2e394f87 => 100
	i32 809851609, ; 46: System.Drawing.Common.dll => 0x30455ad9 => 99
	i32 843511501, ; 47: Xamarin.AndroidX.Print => 0x3246f6cd => 70
	i32 877678880, ; 48: System.Globalization.dll => 0x34505120 => 115
	i32 882883187, ; 49: Xamarin.Android.Support.v4.dll => 0x349fba73 => 29
	i32 902159924, ; 50: Rg.Plugins.Popup => 0x35c5de34 => 15
	i32 913382283, ; 51: Plugin.Settings => 0x36711b8b => 13
	i32 928116545, ; 52: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 90
	i32 954320159, ; 53: ZXing.Net.Mobile.Forms => 0x38e1c51f => 93
	i32 955402788, ; 54: Newtonsoft.Json => 0x38f24a24 => 11
	i32 958213972, ; 55: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 28
	i32 967690846, ; 56: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 56
	i32 974778368, ; 57: FormsViewGroup.dll => 0x3a19f000 => 7
	i32 992768348, ; 58: System.Collections.dll => 0x3b2c715c => 107
	i32 1012816738, ; 59: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 72
	i32 1035644815, ; 60: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 34
	i32 1042160112, ; 61: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 87
	i32 1052210849, ; 62: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 60
	i32 1060935179, ; 63: BMSMobile.dll => 0x3f3c960b => 4
	i32 1098259244, ; 64: System => 0x41761b2c => 17
	i32 1134191450, ; 65: ZXingNetMobile.dll => 0x439a635a => 95
	i32 1175144683, ; 66: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 77
	i32 1178241025, ; 67: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 67
	i32 1204270330, ; 68: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 35
	i32 1267360935, ; 69: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 78
	i32 1293217323, ; 70: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 49
	i32 1364015309, ; 71: System.IO => 0x514d38cd => 113
	i32 1365406463, ; 72: System.ServiceModel.Internals.dll => 0x516272ff => 104
	i32 1376866003, ; 73: Xamarin.AndroidX.SavedState => 0x52114ed3 => 72
	i32 1395363092, ; 74: Plugin.Settings.dll => 0x532b8d14 => 13
	i32 1395857551, ; 75: Xamarin.AndroidX.Media.dll => 0x5333188f => 64
	i32 1406073936, ; 76: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 44
	i32 1445445088, ; 77: Xamarin.Android.Support.Fragment => 0x5627bde0 => 27
	i32 1457743152, ; 78: System.Runtime.Extensions.dll => 0x56e36530 => 108
	i32 1460219004, ; 79: Xamarin.Forms.Xaml => 0x57092c7c => 88
	i32 1462112819, ; 80: System.IO.Compression.dll => 0x57261233 => 100
	i32 1469204771, ; 81: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 33
	i32 1543031311, ; 82: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 114
	i32 1565248321, ; 83: Plugin.Settings.Abstractions => 0x5d4bcb41 => 12
	i32 1571005899, ; 84: zxing.portable => 0x5da3a5cb => 94
	i32 1574652163, ; 85: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 26
	i32 1582372066, ; 86: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 48
	i32 1592978981, ; 87: System.Runtime.Serialization.dll => 0x5ef2ee25 => 3
	i32 1622152042, ; 88: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 62
	i32 1624863272, ; 89: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 81
	i32 1636350590, ; 90: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 46
	i32 1639515021, ; 91: System.Net.Http.dll => 0x61b9038d => 2
	i32 1639986890, ; 92: System.Text.RegularExpressions => 0x61c036ca => 114
	i32 1657153582, ; 93: System.Runtime => 0x62c6282e => 20
	i32 1658241508, ; 94: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 75
	i32 1658251792, ; 95: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 89
	i32 1670060433, ; 96: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 43
	i32 1701541528, ; 97: System.Diagnostics.Debug.dll => 0x656b7698 => 109
	i32 1729485958, ; 98: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 39
	i32 1766324549, ; 99: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 74
	i32 1776026572, ; 100: System.Core.dll => 0x69dc03cc => 16
	i32 1788241197, ; 101: Xamarin.AndroidX.Fragment => 0x6a96652d => 51
	i32 1808609942, ; 102: Xamarin.AndroidX.Loader => 0x6bcd3296 => 62
	i32 1813201214, ; 103: Xamarin.Google.Android.Material => 0x6c13413e => 89
	i32 1818569960, ; 104: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 68
	i32 1867746548, ; 105: Xamarin.Essentials.dll => 0x6f538cf4 => 82
	i32 1878053835, ; 106: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 88
	i32 1885316902, ; 107: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 36
	i32 1898262354, ; 108: dotMorten.Xamarin.Forms.AutoSuggestBox.dll => 0x71252f52 => 5
	i32 1904184254, ; 109: FastAndroidCamera => 0x717f8bbe => 6
	i32 1919157823, ; 110: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 65
	i32 2019465201, ; 111: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 60
	i32 2055257422, ; 112: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 57
	i32 2079903147, ; 113: System.Runtime.dll => 0x7bf8cdab => 20
	i32 2090596640, ; 114: System.Numerics.Vectors => 0x7c9bf920 => 19
	i32 2097448633, ; 115: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 53
	i32 2126786730, ; 116: Xamarin.Forms.Platform.Android => 0x7ec430aa => 86
	i32 2166116741, ; 117: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 26
	i32 2193016926, ; 118: System.ObjectModel.dll => 0x82b6c85e => 110
	i32 2201231467, ; 119: System.Net.Http => 0x8334206b => 2
	i32 2217644978, ; 120: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 77
	i32 2244775296, ; 121: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 63
	i32 2256548716, ; 122: Xamarin.AndroidX.MultiDex => 0x8680336c => 65
	i32 2261435625, ; 123: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 55
	i32 2279755925, ; 124: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 71
	i32 2315684594, ; 125: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 31
	i32 2329204181, ; 126: zxing.portable.dll => 0x8ad4d5d5 => 94
	i32 2330457430, ; 127: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 25
	i32 2341995103, ; 128: ZXingNetMobile => 0x8b98025f => 95
	i32 2373288475, ; 129: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 27
	i32 2409053734, ; 130: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 69
	i32 2431243866, ; 131: ZXing.Net.Mobile.Core.dll => 0x90e9d65a => 91
	i32 2454642406, ; 132: System.Text.Encoding.dll => 0x924edee6 => 112
	i32 2465532216, ; 133: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 42
	i32 2471841756, ; 134: netstandard.dll => 0x93554fdc => 1
	i32 2475788418, ; 135: Java.Interop.dll => 0x93918882 => 8
	i32 2482213323, ; 136: ZXing.Net.Mobile.Forms.Android => 0x93f391cb => 92
	i32 2501346920, ; 137: System.Data.DataSetExtensions => 0x95178668 => 98
	i32 2505896520, ; 138: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 59
	i32 2581819634, ; 139: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 78
	i32 2620871830, ; 140: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 46
	i32 2624373537, ; 141: BMSMobile.Android => 0x9c6cc321 => 0
	i32 2624644809, ; 142: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 50
	i32 2633051222, ; 143: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 58
	i32 2693849962, ; 144: System.IO.dll => 0xa090e36a => 113
	i32 2701096212, ; 145: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 75
	i32 2715334215, ; 146: System.Threading.Tasks.dll => 0xa1d8b647 => 106
	i32 2732626843, ; 147: Xamarin.AndroidX.Activity => 0xa2e0939b => 30
	i32 2737747696, ; 148: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 33
	i32 2766581644, ; 149: Xamarin.Forms.Core => 0xa4e6af8c => 84
	i32 2768457651, ; 150: PropertyChanged => 0xa5034fb3 => 14
	i32 2778768386, ; 151: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 80
	i32 2810250172, ; 152: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 44
	i32 2819470561, ; 153: System.Xml.dll => 0xa80db4e1 => 21
	i32 2853208004, ; 154: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 80
	i32 2855708567, ; 155: Xamarin.AndroidX.Transition => 0xaa36a797 => 76
	i32 2861816565, ; 156: Rg.Plugins.Popup.dll => 0xaa93daf5 => 15
	i32 2903344695, ; 157: System.ComponentModel.Composition => 0xad0d8637 => 102
	i32 2905242038, ; 158: mscorlib.dll => 0xad2a79b6 => 10
	i32 2916838712, ; 159: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 81
	i32 2919462931, ; 160: System.Numerics.Vectors.dll => 0xae037813 => 19
	i32 2921128767, ; 161: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 32
	i32 2978675010, ; 162: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 49
	i32 3024354802, ; 163: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 54
	i32 3033570130, ; 164: dotMorten.Xamarin.Forms.AutoSuggestBox => 0xb4d09b52 => 5
	i32 3044182254, ; 165: FormsViewGroup => 0xb57288ee => 7
	i32 3057625584, ; 166: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 66
	i32 3075834255, ; 167: System.Threading.Tasks => 0xb755818f => 106
	i32 3092211740, ; 168: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 28
	i32 3111772706, ; 169: System.Runtime.Serialization => 0xb979e222 => 3
	i32 3204380047, ; 170: System.Data.dll => 0xbefef58f => 96
	i32 3211777861, ; 171: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 48
	i32 3220365878, ; 172: System.Threading => 0xbff2e236 => 111
	i32 3247949154, ; 173: Mono.Security => 0xc197c562 => 105
	i32 3258312781, ; 174: Xamarin.AndroidX.CardView => 0xc235e84d => 39
	i32 3267021929, ; 175: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 37
	i32 3299363146, ; 176: System.Text.Encoding => 0xc4a8494a => 112
	i32 3317135071, ; 177: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 47
	i32 3317144872, ; 178: System.Data => 0xc5b79d28 => 96
	i32 3340431453, ; 179: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 36
	i32 3346324047, ; 180: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 67
	i32 3353484488, ; 181: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 53
	i32 3362522851, ; 182: Xamarin.AndroidX.Core => 0xc86c06e3 => 45
	i32 3366347497, ; 183: Java.Interop => 0xc8a662e9 => 8
	i32 3374999561, ; 184: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 71
	i32 3404865022, ; 185: System.ServiceModel.Internals => 0xcaf21dfe => 104
	i32 3429136800, ; 186: System.Xml => 0xcc6479a0 => 21
	i32 3430777524, ; 187: netstandard => 0xcc7d82b4 => 1
	i32 3439690031, ; 188: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 23
	i32 3441283291, ; 189: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 50
	i32 3476120550, ; 190: Mono.Android => 0xcf3163e6 => 9
	i32 3486231360, ; 191: Plugin.Settings.Abstractions.dll => 0xcfcbab40 => 12
	i32 3486566296, ; 192: System.Transactions => 0xcfd0c798 => 97
	i32 3493954962, ; 193: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 41
	i32 3501239056, ; 194: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 37
	i32 3509114376, ; 195: System.Xml.Linq => 0xd128d608 => 22
	i32 3536029504, ; 196: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 86
	i32 3567349600, ; 197: System.ComponentModel.Composition.dll => 0xd4a16f60 => 102
	i32 3618140916, ; 198: Xamarin.AndroidX.Preference => 0xd7a872f4 => 69
	i32 3627220390, ; 199: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 70
	i32 3632359727, ; 200: Xamarin.Forms.Platform => 0xd881692f => 87
	i32 3633644679, ; 201: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 32
	i32 3641597786, ; 202: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 57
	i32 3645376713, ; 203: Xamarin.Forms.BehaviorsPack.dll => 0xd94808c9 => 83
	i32 3672681054, ; 204: Mono.Android.dll => 0xdae8aa5e => 9
	i32 3676310014, ; 205: System.Web.Services.dll => 0xdb2009fe => 103
	i32 3682565725, ; 206: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 38
	i32 3684561358, ; 207: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 41
	i32 3685813676, ; 208: Xamarin.Forms.Material.dll => 0xdbb10dac => 85
	i32 3689375977, ; 209: System.Drawing.Common => 0xdbe768e9 => 99
	i32 3718780102, ; 210: Xamarin.AndroidX.Annotation => 0xdda814c6 => 31
	i32 3724971120, ; 211: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 66
	i32 3758932259, ; 212: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 55
	i32 3786282454, ; 213: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 40
	i32 3822602673, ; 214: Xamarin.AndroidX.Media => 0xe3d849b1 => 64
	i32 3829621856, ; 215: System.Numerics.dll => 0xe4436460 => 18
	i32 3847036339, ; 216: ZXing.Net.Mobile.Forms.Android.dll => 0xe54d1db3 => 92
	i32 3885922214, ; 217: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 76
	i32 3896760992, ; 218: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 45
	i32 3920810846, ; 219: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 101
	i32 3921031405, ; 220: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 79
	i32 3930341074, ; 221: BMSMobile => 0xea443ed2 => 4
	i32 3931092270, ; 222: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 68
	i32 3945713374, ; 223: System.Data.DataSetExtensions.dll => 0xeb2ecede => 98
	i32 3955647286, ; 224: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 34
	i32 4073602200, ; 225: System.Threading.dll => 0xf2ce3c98 => 111
	i32 4105002889, ; 226: Mono.Security.dll => 0xf4ad5f89 => 105
	i32 4151237749, ; 227: System.Core => 0xf76edc75 => 16
	i32 4182413190, ; 228: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 61
	i32 4184000013, ; 229: PropertyChanged.dll => 0xf962c60d => 14
	i32 4186595366, ; 230: ZXing.Net.Mobile.Core => 0xf98a6026 => 91
	i32 4292120959 ; 231: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 61
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [232 x i32] [
	i32 59, i32 90, i32 11, i32 84, i32 73, i32 73, i32 40, i32 24, ; 0..7
	i32 6, i32 74, i32 83, i32 38, i32 29, i32 0, i32 109, i32 54, ; 8..15
	i32 115, i32 103, i32 43, i32 58, i32 52, i32 30, i32 18, i32 93, ; 16..23
	i32 56, i32 85, i32 25, i32 42, i32 82, i32 107, i32 51, i32 10, ; 24..31
	i32 17, i32 52, i32 63, i32 110, i32 24, i32 97, i32 108, i32 101, ; 32..39
	i32 47, i32 79, i32 35, i32 22, i32 23, i32 100, i32 99, i32 70, ; 40..47
	i32 115, i32 29, i32 15, i32 13, i32 90, i32 93, i32 11, i32 28, ; 48..55
	i32 56, i32 7, i32 107, i32 72, i32 34, i32 87, i32 60, i32 4, ; 56..63
	i32 17, i32 95, i32 77, i32 67, i32 35, i32 78, i32 49, i32 113, ; 64..71
	i32 104, i32 72, i32 13, i32 64, i32 44, i32 27, i32 108, i32 88, ; 72..79
	i32 100, i32 33, i32 114, i32 12, i32 94, i32 26, i32 48, i32 3, ; 80..87
	i32 62, i32 81, i32 46, i32 2, i32 114, i32 20, i32 75, i32 89, ; 88..95
	i32 43, i32 109, i32 39, i32 74, i32 16, i32 51, i32 62, i32 89, ; 96..103
	i32 68, i32 82, i32 88, i32 36, i32 5, i32 6, i32 65, i32 60, ; 104..111
	i32 57, i32 20, i32 19, i32 53, i32 86, i32 26, i32 110, i32 2, ; 112..119
	i32 77, i32 63, i32 65, i32 55, i32 71, i32 31, i32 94, i32 25, ; 120..127
	i32 95, i32 27, i32 69, i32 91, i32 112, i32 42, i32 1, i32 8, ; 128..135
	i32 92, i32 98, i32 59, i32 78, i32 46, i32 0, i32 50, i32 58, ; 136..143
	i32 113, i32 75, i32 106, i32 30, i32 33, i32 84, i32 14, i32 80, ; 144..151
	i32 44, i32 21, i32 80, i32 76, i32 15, i32 102, i32 10, i32 81, ; 152..159
	i32 19, i32 32, i32 49, i32 54, i32 5, i32 7, i32 66, i32 106, ; 160..167
	i32 28, i32 3, i32 96, i32 48, i32 111, i32 105, i32 39, i32 37, ; 168..175
	i32 112, i32 47, i32 96, i32 36, i32 67, i32 53, i32 45, i32 8, ; 176..183
	i32 71, i32 104, i32 21, i32 1, i32 23, i32 50, i32 9, i32 12, ; 184..191
	i32 97, i32 41, i32 37, i32 22, i32 86, i32 102, i32 69, i32 70, ; 192..199
	i32 87, i32 32, i32 57, i32 83, i32 9, i32 103, i32 38, i32 41, ; 200..207
	i32 85, i32 99, i32 31, i32 66, i32 55, i32 40, i32 64, i32 18, ; 208..215
	i32 92, i32 76, i32 45, i32 101, i32 79, i32 4, i32 68, i32 98, ; 216..223
	i32 34, i32 111, i32 105, i32 16, i32 61, i32 14, i32 91, i32 61 ; 232..231
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"NumRegisterParameters", i32 0}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ a200af12c1e846626b8caebd926ac14c185f71ec"}
!llvm.linker.options = !{}
