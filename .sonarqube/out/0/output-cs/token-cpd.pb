§
eD:\Repos\aspnetcore-jsonlocalizer\src\Cms.AspNetCore.JsonLocalizer\Options\JsonLocalizationOptions.cs
	namespace 	
Cms
 
. 

AspNetCore 
. 
JsonLocalizer &
.& '
Options' .
;. /
public 
class #
JsonLocalizationOptions $
{ 
public 

string 
ResourcesPath 
{  !
get" %
;% &
set' *
;* +
}, -
=. /
$str0 ;
;; <
public 

string 
? 
DefaultCulture !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
$str2 9
;9 :
} Ð
hD:\Repos\aspnetcore-jsonlocalizer\src\Cms.AspNetCore.JsonLocalizer\Infrastructure\JsonStringLocalizer.cs
	namespace 	
Cms
 
. 

AspNetCore 
. 
JsonLocalizer &
.& '
Infrastructure' 5
;5 6
public 
class 
JsonStringLocalizer  
(  !(
JsonLocalizationFileAccessor! =
accessor> F
,F G
stringH N
cultureO V
)V W
:X Y 
IJsonStringLocalizerZ n
{ 
private 
readonly 
string 
_culture $
=% &
culture' .
;. /
private 
readonly (
JsonLocalizationFileAccessor 1
	_accessor2 ;
=< =
accessor> F
;F G
public 

string 
this 
[ 
string 
key !
]! "
=># %
Format& ,
(, -
key- 0
,0 1
[2 3
]3 4
)4 5
;5 6
public 

string 
this 
[ 
string 
key !
,! "
params# )
object* 0
[0 1
]1 2
	arguments3 <
]< =
=>> @
FormatA G
(G H
keyH K
,K L
	argumentsM V
)V W
;W X
private 
string 
Format 
( 
string  
key! $
,$ %
object& ,
[, -
]- .
args/ 3
)3 4
{ 
var 
template 
= 
	_accessor  
.  !
GetValue! )
() *
key* -
,- .
_culture/ 7
)7 8
;8 9
return 
string 
. 
Format 
( 
CultureInfo (
.( )
CurrentCulture) 7
,7 8
template9 A
??B D
keyE H
,H I
argsJ N
)N O
;O P
} 
} •
qD:\Repos\aspnetcore-jsonlocalizer\src\Cms.AspNetCore.JsonLocalizer\Infrastructure\JsonLocalizationFileAccessor.cs
	namespace 	
Cms
 
. 

AspNetCore 
. 
JsonLocalizer &
.& '
Infrastructure' 5
;5 6
public		 
class		 (
JsonLocalizationFileAccessor		 )
(		) *
string		* 0
basePath		1 9
)		9 :
{

 
private 
readonly 
string 
	_basePath %
=& '
basePath( 0
;0 1
private 
readonly 

Dictionary 
<  
string  &
,& '
JsonNode( 0
?0 1
>1 2
_cache3 9
=: ;
[< =
]= >
;> ?
public 

string 
? 
GetValue 
( 
string "
key# &
,& '
string( .
culture/ 6
)6 7
{ 
if 

( 
! 
_cache 
. 
TryGetValue 
(  
culture  '
,' (
out) ,
JsonNode- 5
?5 6
root7 ;
); <
)< =
{ 	
var 
file 
= 
Path 
. 
Combine #
(# $
	_basePath$ -
,- .
$"/ 1
{1 2
culture2 9
}9 :
$str: ?
"? @
)@ A
;A B
if 
( 
! 
File 
. 
Exists 
( 
file !
)! "
)" #
{ 
return 
null 
; 
} 
root!! 
=!! 
JsonNode!! 
.!! 
Parse!! !
(!!! "
File!!" &
.!!& '
ReadAllText!!' 2
(!!2 3
file!!3 7
)!!7 8
)!!8 9
;!!9 :
_cache"" 
["" 
culture"" 
]"" 
="" 
root"" "
;""" #
}## 	
return%% 
Traverse%% 
(%% 
root%% 
,%% 
key%% !
.%%! "
Split%%" '
(%%' (
$char%%( +
)%%+ ,
)%%, -
?%%- .
.%%. /
ToString%%/ 7
(%%7 8
)%%8 9
;%%9 :
}&& 
private00 
static00 
JsonNode00 
?00 
Traverse00 %
(00% &
JsonNode00& .
?00. /
node000 4
,004 5
string006 <
[00< =
]00= >
parts00? D
)00D E
{11 
foreach22 
(22 
var22 
part22 
in22 
parts22 "
)22" #
{33 	
if44 
(44 
node44 
is44 

JsonObject44 "
obj44# &
&&44' )
obj44* -
.44- .
TryGetPropertyValue44. A
(44A B
part44B F
,44F G
out44H K
JsonNode44L T
?44T U
next44V Z
)44Z [
)44[ \
{55 
node66 
=66 
next66 
;66 
}77 
else88 
{99 
return:: 
null:: 
;:: 
};; 
}<< 	
return== 
node== 
;== 
}>> 
}?? Ð
lD:\Repos\aspnetcore-jsonlocalizer\src\Cms.AspNetCore.JsonLocalizer\Extensions\ServiceCollectionExtensions.cs
	namespace		 	
Cms		
 
.		 

AspNetCore		 
.		 
JsonLocalizer		 &
.		& '

Extensions		' 1
;		1 2
public 
static 
class '
ServiceCollectionExtensions /
{ 
public 

static 
IServiceCollection $
AddJsonLocalization% 8
(8 9
this9 =
IServiceCollection> P
servicesQ Y
,Y Z
Action 
< #
JsonLocalizationOptions &
>& '
	configure( 1
)1 2
{ 
services 
. 
	Configure 
( 
	configure $
)$ %
;% &
services 
. "
AddHttpContextAccessor '
(' (
)( )
;) *
services   
.   
AddSingleton   
(   
sp    
=>  ! #
{!! 	#
JsonLocalizationOptions"" #
options""$ +
="", -
sp"". 0
.""0 1
GetRequiredService""1 C
<""C D
IOptions""D L
<""L M#
JsonLocalizationOptions""M d
>""d e
>""e f
(""f g
)""g h
.""h i
Value""i n
;""n o
return## 
new## (
JsonLocalizationFileAccessor## 3
(##3 4
options##4 ;
.##; <
ResourcesPath##< I
)##I J
;##J K
}$$ 	
)$$	 

;$$
 
services'' 
.'' 
	AddScoped'' 
<''  
IJsonStringLocalizer'' /
>''/ 0
(''0 1
sp''1 3
=>''4 6
{(( 	(
JsonLocalizationFileAccessor)) (
accessor))) 1
=))2 3
sp))4 6
.))6 7
GetRequiredService))7 I
<))I J(
JsonLocalizationFileAccessor))J f
>))f g
())g h
)))h i
;))i j
HttpContext** 
httpContext** #
=**$ %
sp**& (
.**( )
GetRequiredService**) ;
<**; < 
IHttpContextAccessor**< P
>**P Q
(**Q R
)**R S
.**S T
HttpContext**T _
;**_ `#
JsonLocalizationOptions++ #
options++$ +
=++, -
sp++. 0
.++0 1
GetRequiredService++1 C
<++C D
IOptions++D L
<++L M#
JsonLocalizationOptions++M d
>++d e
>++e f
(++f g
)++g h
.++h i
Value++i n
;++n o
var.. 
acceptLanguage.. 
=..  
httpContext..! ,
?.., -
...- .
Request... 5
...5 6
Headers..6 =
[..= >
$str..> O
]..O P
...P Q
FirstOrDefault..Q _
(.._ `
)..` a
;..a b
var// 
culture// 
=// 
!// 
string// !
.//! "
IsNullOrWhiteSpace//" 4
(//4 5
acceptLanguage//5 C
)//C D
?00 
acceptLanguage00  
:11 
options11 
.11 
DefaultCulture11 (
??11) +
CultureInfo11, 7
.117 8
CurrentCulture118 F
.11F G
Name11G K
;11K L
return33 
new33 
JsonStringLocalizer33 *
(33* +
accessor33+ 3
,333 4
culture335 <
)33< =
;33= >
}44 	
)44	 

;44
 
return66 
services66 
;66 
}77 
}88 ·
gD:\Repos\aspnetcore-jsonlocalizer\src\Cms.AspNetCore.JsonLocalizer\Abstractions\IJsonStringLocalizer.cs
	namespace 	
Cms
 
. 

AspNetCore 
. 
JsonLocalizer &
.& '
Abstractions' 3
;3 4
public 
	interface  
IJsonStringLocalizer %
{ 
string 

this 
[ 
string 
key 
] 
{ 
get !
;! "
}# $
string## 

this## 
[## 
string## 
key## 
,## 
params## "
object### )
[##) *
]##* +
	arguments##, 5
]##5 6
{##7 8
get##9 <
;##< =
}##> ?
}$$ 