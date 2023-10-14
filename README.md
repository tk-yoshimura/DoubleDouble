# DoubleDouble
 Double-Double Arithmetic and Special Function Implements 
 
## Requirement
.NET 7.0

## Install

[Download DLL](https://github.com/tk-yoshimura/DoubleDouble/releases)  
[Download Nuget](https://www.nuget.org/packages/tyoshimura.doubledouble/)  

## More Precision ?
[MultiPrecision](https://github.com/tk-yoshimura/MultiPrecision)  

## Type

|type|mantissa bits|significant digits|
|----|----|----|
|ddouble|104|30|

Epsilon: 2^-968 = 4.00833e-292  
MaxValue : 2^1024 = 1.79769e308

## Functions

|function|domain|mantissa error bits|note|
|----|----|----|----|
|ddouble.Sqrt(x)|&#91;0,+inf&#41;|2||
|ddouble.Cbrt(x)|&#40;-inf,+inf&#41;|2||
|ddouble.RootN(x, n)|&#40;-inf,+inf&#41;|3||
|ddouble.Log2(x)|&#40;0,+inf&#41;|2||
|ddouble.Log(x), ddouble.Log(x, b)|&#40;0,+inf&#41;|3||
|ddouble.Log10(x)|&#40;0,+inf&#41;|3||
|ddouble.Log1p(x)|&#40;-1,+inf&#41;|3|log(1+x)|
|ddouble.Pow2(x)|&#40;-inf,+inf&#41;|1||
|ddouble.Pow2m1(x)|&#40;-inf,+inf&#41;|2|pow2(x)-1|
|ddouble.Pow(x, y)|&#40;-inf,+inf&#41;|2||
|ddouble.Pow10(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Exp(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Expm1(x)|&#40;-inf,+inf&#41;|2|exp(x)-1|
|ddouble.Sin(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Cos(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Tan(x)|&#40;-inf,+inf&#41;|3||
|ddouble.SinPI(x)|&#40;-inf,+inf&#41;|1| sin(&pi;x) |
|ddouble.CosPI(x)|&#40;-inf,+inf&#41;|1| cos(&pi;x) |
|ddouble.TanPI(x)|&#40;-inf,+inf&#41;|2| tan(&pi;x) |
|ddouble.Sinh(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Cosh(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Tanh(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Asin(x)|&#91;-1,1&#93;|2|Accuracy deteriorates near x=-1,1.|
|ddouble.Acos(x)|&#91;-1,1&#93;|2|Accuracy deteriorates near x=-1,1.|
|ddouble.Atan(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Atan2(y, x)|&#40;-inf,+inf&#41;|2||
|ddouble.Arsinh(x)|&#40;-inf,+inf&#41;|2||
|ddouble.Arcosh(x)|&#91;1,+inf&#41;|2||
|ddouble.Artanh(x)|&#40;-1,1&#41;|4|Accuracy deteriorates near x=-1,1.|
|ddouble.Sinc(x, normalized)|&#40;-inf,+inf&#41;|2||
|ddouble.Sinhc(x)|&#40;-inf,+inf&#41;|3||
|ddouble.Gamma(x)|&#40;-inf,+inf&#41;|2|Accuracy deteriorates near non-positive intergers. <br/> If x is Natual number lass than 35, an exact integer value is returned. |
|ddouble.LogGamma(x)|&#40;0,+inf&#41;|4||
|ddouble.Digamma(x)|&#40;-inf,+inf&#41;|4|Near the positive root, polynomial interpolation is used.|
|ddouble.Polygamma(n, x)|&#40;-inf,+inf&#41;|4|Accuracy deteriorates near non-positive intergers. <br/> n &leq; 16|
|ddouble.InverseGamma(x)|&#91;1,+inf&#41;|2|gamma^-1(x)|
|ddouble.LowerIncompleteGamma(nu, x)|&#91;0,+inf&#41;|4|nu &leq; 171.625|
|ddouble.UpperIncompleteGamma(nu, x)|&#91;0,+inf&#41;|4|nu &leq; 171.625|
|ddouble.LowerIncompleteGammaRegularized(nu, x)|&#91;0,+inf&#41;|4|nu &leq; 8192|
|ddouble.UpperIncompleteGammaRegularized(nu, x)|&#91;0,+inf&#41;|4|nu &leq; 8192|
|ddouble.InverseLowerIncompleteGamma(nu, x)|&#91;0,1&#93;|8|nu &leq; 8192|
|ddouble.InverseUpperIncompleteGamma(nu, x)|&#91;0,1&#93;|8|nu &leq; 8192|
|ddouble.Beta(a, b)|&#91;0,+inf&#41;|4||
|ddouble.LogBeta(a, b)|&#91;0,+inf&#41;|4||
|ddouble.IncompleteBeta(x, a, b)|&#91;0,1&#93;|4|Accuracy decreases when the radio of a,b is too large. a+b-max(a,b) &leq; 512|
|ddouble.IncompleteBetaRegularized(x, a, b)|&#91;0,1&#93;|4|Accuracy decreases when the radio of a,b is too large. a+b-max(a,b) &leq; 8192|
|ddouble.InverseIncompleteBeta(x, a, b)|&#91;0,1&#93;|8|Accuracy decreases when the radio of a,b is too large. a+b-max(a,b) &leq; 8192|
|ddouble.Erf(x)|&#40;-inf,+inf&#41;|3||
|ddouble.Erfc(x)|&#40;-inf,+inf&#41;|3||
|ddouble.InverseErf(x)|&#40;-1,1&#41;|3||
|ddouble.InverseErfc(x)|&#40;0,2&#41;|3||
|ddouble.Erfcx(x)|&#40;-inf,+inf&#41;|3||
|ddouble.Erfi(x)|&#40;-inf,+inf&#41;|4||
|ddouble.DawsonF(x)|&#40;-inf,+inf&#41;|4||
|ddouble.BesselJ(nu, x)|&#91;0,+inf&#41;|8|Accuracy deteriorates near root.<br/>abs(nu) &leq; 16 |
|ddouble.BesselY(nu, x)|&#91;0,+inf&#41;|8|Accuracy deteriorates near the root and <br/>at non-interger nu very close (&lt; 2^-25) to the integer.<br/>abs(nu) &leq; 16 |
|ddouble.BesselI(nu, x)|&#91;0,+inf&#41;|6|Accuracy deteriorates near root.<br/>abs(nu) &leq; 16 |
|ddouble.BesselK(nu, x)|&#91;0,+inf&#41;|6|Accuracy deteriorates with non-interger nu very close <br/> (&lt; 2^-25) to an integer.<br/>abs(nu) &leq; 16 |
|ddouble.StruveH(n, x)|&#40;-inf,+inf&#41;|4|0 &leq; n &leq; 8|
|ddouble.StruveK(n, x)|&#91;0,+inf&#41;|4|0 &leq; n &leq; 8|
|ddouble.StruveL(n, x)|&#40;-inf,+inf&#41;|4|0 &leq; n &leq; 8|
|ddouble.StruveM(n, x)|&#91;0,+inf&#41;|4|0 &leq; n &leq; 8|
|ddouble.EllipticK(m)|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.EllipticE(m)|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.EllipticPi(n, m)|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.EllipticK(x, m)|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.EllipticE(x, m)|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2, incomplete elliptic integral|
|ddouble.EllipticPi(n, x, m)|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2<br/>Argument order follows wolfram. incomplete elliptic integral|
|ddouble.EllipticTheta(a, x, q)|&#40;-inf,+inf&#41;|4|a=1...4, q &leq; 0.995, incomplete elliptic integral|
|ddouble.KeplerE(m, e, centered)|&#40;-inf,+inf&#41;|6|inverse kepler's equation, e(eccentricity) &leq; 256|
|ddouble.Agm(a, b)|&#91;0,+inf&#41;|2||
|ddouble.FresnelC(x)|&#40;-inf,+inf&#41;|4||
|ddouble.FresnelS(x)|&#40;-inf,+inf&#41;|4||
|ddouble.FresnelF(x)|&#40;-inf,+inf&#41;|4||
|ddouble.FresnelG(x)|&#40;-inf,+inf&#41;|4||
|ddouble.Ei(x)|&#40;-inf,+inf&#41;|4|exponential integral|
|ddouble.Ein(x)|&#40;-inf,+inf&#41;|4|complementary exponential integral|
|ddouble.Li(x)|&#91;0,+inf&#41;|5|logarithmic integral li(x)=ei(log(x))|
|ddouble.Si(x, limit_zero)|&#40;-inf,+inf&#41;|4|sin integral, limit_zero=true: si(x)|
|ddouble.Ci(x)|&#91;0,+inf&#41;|4|cos integral|
|ddouble.Shi(x)|&#40;-inf,+inf&#41;|5|hyperbolic sin integral|
|ddouble.Chi(x)|&#91;0,+inf&#41;|5|hyperbolic cos integral|
|ddouble.LambertW(x)|&#91;-1/e,+inf&#41;|4||
|ddouble.AiryAi(x)|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near root.|
|ddouble.AiryBi(x)|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near root.|
|ddouble.JacobiSn(x, m)|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiCn(x, m)|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiDn(x, m)|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiAm(x, m)|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiArcSn(x, m)|&#91;-1,+1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiArcCn(x, m)|&#91;-1,+1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.JacobiArcDn(x, m)|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|
|ddouble.CarlsonRD(x, y, z)|&#91;0,+inf&#41;|4||
|ddouble.CarlsonRC(x, y)|&#91;0,+inf&#41;|4||
|ddouble.CarlsonRF(x, y, z)|&#91;0,+inf&#41;|4||
|ddouble.CarlsonRJ(x, y, z, w)|&#91;0,+inf&#41;|4||
|ddouble.CarlsonRG(x, y, z)|&#91;0,+inf&#41;|4||
|ddouble.RiemannZeta(x)|&#40;-inf,+inf&#41;|3||
|ddouble.HurwitzZeta(x, a)|&#40;1,+inf&#41;|3|a &geq; 0|
|ddouble.DirichletEta(x)|&#40;-inf,+inf&#41;|3||
|ddouble.Polylog(n, x)|&#40;-inf,1&#93;|3|n &in; &#91;-4,8&#93;|
|ddouble.OwenT(h, a)|&#40;-inf,+inf&#41;|5||
|ddouble.Bump(x)|&#40;-inf,+inf&#41;|4|C-infinity smoothness basis function, bump(x)=1/(exp(1/x-1/(1-x))+1)|
|ddouble.HermiteH(n, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.LaguerreL(n, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.LaguerreL(n, alpha, x)|&#40;-inf,+inf&#41;|3|n &leq; 64, associated|
|ddouble.LegendreP(n, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.LegendreP(n, m, x)|&#91;-1,1&#93;|3|n &leq; 64, associated|
|ddouble.ChebyshevT(n, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.ChebyshevU(n, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.ZernikeR(n, m, x)|&#91;0,1&#93;|3|n &leq; 64|
|ddouble.GegenbauerC(n, alpha, x)|&#40;-inf,+inf&#41;|3|n &leq; 64|
|ddouble.JacobiP(n, alpha, beta, x)|&#91;-1,1&#93;|3|n &leq; 64, alpha,beta &gt; -1|
|ddouble.Bernoulli(n, x, centered)|&#91;0,1&#93;|4|n &leq; 64, centered: x->x-1/2|
|ddouble.MathieuA(n, q)|&#40;-inf,+inf&#41;|4|n &leq; 16|
|ddouble.MathieuB(n, q)|&#40;-inf,+inf&#41;|4|n &leq; 16|
|ddouble.MathieuC(n, q, x)|&#40;-inf,+inf&#41;|4|n &leq; 16, Accuracy deteriorates when q is very large.|
|ddouble.MathieuS(n, q, x)|&#40;-inf,+inf&#41;|4|n &leq; 16, Accuracy deteriorates when q is very large.|
|ddouble.Ldexp(x, y)|&#40;-inf,+inf&#41;|N/A||
|ddouble.Binomial(n, k)|N/A|1|n &leq; 1000|
|ddouble.Hypot(x, y)|N/A|2||
|ddouble.Min(x, y)|N/A|N/A||
|ddouble.Max(x, y)|N/A|N/A||
|ddouble.Clamp(v, min, max)|N/A|N/A||
|ddouble.CopySign(value, sign)|N/A|N/A||
|ddouble.Floor(x)|N/A|N/A||
|ddouble.Ceiling(x)|N/A|N/A||
|ddouble.Round(x)|N/A|N/A||
|ddouble.Truncate(x)|N/A|N/A||
|IEnumerable&lt;ddouble&gt;.Sum()|N/A|N/A||
|IEnumerable&lt;ddouble&gt;.Average()|N/A|N/A||
|IEnumerable&lt;ddouble&gt;.Min()|N/A|N/A||
|IEnumerable&lt;ddouble&gt;.Max()|N/A|N/A||

## Constants

|constant|value|note|
|----|----|----|
|ddouble.PI|3.141592653589793238462...|Pi|
|ddouble.E|2.718281828459045235360...|Napier's E|
|ddouble.EulerGamma|0.577215664901532860606...|Euler's Gamma|
|ddouble.Zeta3|1.202056903159594285399...|&zeta;(3), Apery const.|
|ddouble.Zeta5|1.036927755143369926331...|&zeta;(5)|
|ddouble.Zeta7|1.008349277381922826839...|&zeta;(7)|
|ddouble.Zeta9|1.002008392826082214418...|&zeta;(9)|
|ddouble.DigammaZero|1.461632144968362341263...|Positive root of digamma|
|ddouble.ErdosBorwein|1.606695152415291763783...|Erdös Borwein constant|
|ddouble.FeigenbaumDelta|4.669201609102990671853...|Feigenbaum constant|
|ddouble.LemniscatePI|2.622057554292119810465...|Lemniscate constant|

## Sequence

|sequence|note|
|----|----|
|ddouble.TaylorSequence|Taylor,1/n!|
|ddouble.Factorial|Factorial,n!|
|ddouble.BernoulliSequence|Bernoulli,B(2k)|
|ddouble.HarmonicNumber|HarmonicNumber, H_n|
|ddouble.StieltjesGamma|StieltjesGamma, &gamma;_n|

## Casts

- long (accurately)
```csharp
ddouble v0 = 123;
long n0 = (long)v0;
```
- double (accurately)
```csharp
ddouble v1 = 0.5;
double n1 = (double)v1;
```
- decimal (approximately)
```csharp
ddouble v1 = 0.1m;
decimal n1 = (decimal)v1;
```
- string (approximately)
```csharp
ddouble v2 = "3.14e0";
string s0 = v2.ToString();
string s1 = v2.ToString("E8");
string s2 = $"{v2:E8}";
```

## I/O

BinaryWriter, BinaryReader

## Licence
[MIT](https://github.com/tk-yoshimura/DoubleDouble/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
