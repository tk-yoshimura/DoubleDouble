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

|function|domain|mantissa error bits|note|usage|
|----|----|----|----|----|
|sqrt|&#91;0,+inf&#41;|2||ddouble.Sqrt(x)|
|cbrt|&#40;-inf,+inf&#41;|2||ddouble.Cbrt(x)|
|root_n|&#40;-inf,+inf&#41;|3||ddouble.RootN(x, n)|
|log2|&#40;0,+inf&#41;|2||ddouble.Log2(x)|
|log|&#40;0,+inf&#41;|3||ddouble.Log(x), ddouble.Log(x, b)|
|log10|&#40;0,+inf&#41;|3||ddouble.Log10(x)|
|log1p|&#40;-1,+inf&#41;|3|log(1+x)|ddouble.Log1p(x)|
|pow2|&#40;-inf,+inf&#41;|1||ddouble.Pow2(x)|
|pow2m1|&#40;-inf,+inf&#41;|2|pow2(x)-1|ddouble.Pow2m1(x)|
|pow|&#40;-inf,+inf&#41;|2||ddouble.Pow(x, y)|
|pow10|&#40;-inf,+inf&#41;|2||ddouble.Pow10(x)|
|exp|&#40;-inf,+inf&#41;|2||ddouble.Exp(x)|
|expm1|&#40;-inf,+inf&#41;|2|exp(x)-1|ddouble.Expm1(x)|
|sin|&#40;-inf,+inf&#41;|2||ddouble.Sin(x)|
|cos|&#40;-inf,+inf&#41;|2||ddouble.Cos(x)|
|tan|&#40;-inf,+inf&#41;|3||ddouble.Tan(x)|
|sinpi|&#40;-inf,+inf&#41;|1| sin(&pi;x) |ddouble.SinPI(x)|
|cospi|&#40;-inf,+inf&#41;|1| cos(&pi;x) |ddouble.CosPI(x)|
|tanpi|&#40;-inf,+inf&#41;|2| tan(&pi;x) |ddouble.TanPI(x)|
|sinh|&#40;-inf,+inf&#41;|2||ddouble.Sinh(x)|
|cosh|&#40;-inf,+inf&#41;|2||ddouble.Cosh(x)|
|tanh|&#40;-inf,+inf&#41;|2||ddouble.Tanh(x)|
|asin|&#91;-1,1&#93;|2|Accuracy deteriorates near x=-1,1.|ddouble.Asin(x)|
|acos|&#91;-1,1&#93;|2|Accuracy deteriorates near x=-1,1.|ddouble.Acos(x)|
|atan|&#40;-inf,+inf&#41;|2||ddouble.Atan(x)|
|atan2|&#40;-inf,+inf&#41;|2||ddouble.Atan2(y, x)|
|arsinh|&#40;-inf,+inf&#41;|2||ddouble.Arsinh(x)|
|arcosh|&#91;1,+inf&#41;|2||ddouble.Arcosh(x)|
|artanh|&#40;-1,1&#41;|4|Accuracy deteriorates near x=-1,1.|ddouble.Artanh(x)|
|sinc|&#40;-inf,+inf&#41;|2||ddouble.Sinc(x, normalized)|
|sinhc|&#40;-inf,+inf&#41;|3||ddouble.Sinhc(x)|
|gamma|&#40;-inf,+inf&#41;|2|Accuracy deteriorates near non-positive intergers. <br/> If x is Natual number lass than 35, an exact integer value is returned. |ddouble.Gamma(x)|
|loggamma|&#40;0,+inf&#41;|4||ddouble.LogGamma(x)|
|digamma|&#40;-inf,+inf&#41;|4|Near the positive root, polynomial interpolation is used.|ddouble.Digamma(x)|
|polygamma|&#40;-inf,+inf&#41;|4|Accuracy deteriorates near non-positive intergers. <br/> n &leq; 16|ddouble.Polygamma(n, x)|
|inverse_gamma|&#91;1,+inf&#41;|4|gamma^-1(x)|ddouble.InverseGamma(x)|
|lower_incomplete_gamma|&#91;0,+inf&#41;|4|nu &leq; 170.625|ddouble.LowerIncompleteGamma(nu, x)|
|upper_incomplete_gamma|&#91;0,+inf&#41;|4|nu &leq; 170.625|ddouble.UpperIncompleteGamma(nu, x)|
|lower_incomplete_gamma_regularized|&#91;0,+inf&#41;|4|nu &leq; 8192|ddouble.LowerIncompleteGammaRegularized(nu, x)|
|upper_incomplete_gamma_regularized|&#91;0,+inf&#41;|4|nu &leq; 8192|ddouble.UpperIncompleteGammaRegularized(nu, x)|
|beta|&#91;0,+inf&#41;|4||ddouble.Beta(a, b)|
|logbeta|&#91;0,+inf&#41;|4||ddouble.LogBeta(a, b)|
|incomplete_beta|&#91;0,1&#93;|4|Accuracy decreases when the radio of a,b is too large. a+b-max(a,b) &leq; 512|ddouble.IncompleteBeta(x, a, b)|
|incomplete_beta_regularized|&#91;0,1&#93;|4|Accuracy decreases when the radio of a,b is too large. a+b-max(a,b) &leq; 8192|ddouble.IncompleteBetaRegularized(x, a, b)|
|erf|&#40;-inf,+inf&#41;|3||ddouble.Erf(x)|
|erfc|&#40;-inf,+inf&#41;|3||ddouble.Erfc(x)|
|inverse_erf|&#40;-1,1&#41;|3||ddouble.InverseErf(x)|
|inverse_erfc|&#40;0,2&#41;|3||ddouble.InverseErfc(x)|
|erfcx|&#40;-inf,+inf&#41;|3||ddouble.Erfcx(x)|
|erfi|&#40;-inf,+inf&#41;|4||ddouble.Erfi(x)|
|dawson_f|&#40;-inf,+inf&#41;|4||ddouble.DawsonF(x)|
|bessel_j|&#91;0,+inf&#41;|8|Accuracy deteriorates near root.<br/>abs(nu) &leq; 16 |ddouble.BesselJ(nu, x)|
|bessel_y|&#91;0,+inf&#41;|8|Accuracy deteriorates near the root and <br/>at non-interger nu very close (&lt; 2^-25) to the integer.<br/>abs(nu) &leq; 16 |ddouble.BesselY(nu, x)|
|bessel_i|&#91;0,+inf&#41;|6|Accuracy deteriorates near root.<br/>abs(nu) &leq; 16 |ddouble.BesselI(nu, x)|
|bessel_k|&#91;0,+inf&#41;|6|Accuracy deteriorates with non-interger nu very close <br/> (&lt; 2^-25) to an integer.<br/>abs(nu) &leq; 16 |ddouble.BesselK(nu, x)|
|struve_h|&#40;-inf,+inf&#41;|4|0 &leq; n &leq; 8|ddouble.StruveH(n, x)|
|struve_k|&#91;0,+inf&#41;|4|0 &leq; n &leq; 8|ddouble.StruveK(n, x)|
|struve_l|&#40;-inf,+inf&#41;|4|0 &leq; n &leq; 8|ddouble.StruveL(n, x)|
|struve_m|&#91;0,+inf&#41;|4|0 &leq; n &leq; 8|ddouble.StruveM(n, x)|
|elliptic_k|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticK(m)|
|elliptic_e|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticE(m)|
|elliptic_pi|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticPi(n, m)|
|incomplete_elliptic_k|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticK(x, m)|
|incomplete_elliptic_e|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticE(x, m)|
|incomplete_elliptic_pi|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2<br/>Argument order follows wolfram.|ddouble.EllipticPi(n, x, m)|
|elliptic_theta|&#40;-inf,+inf&#41;|4|a=1...4, q &leq; 0.995|ddouble.EllipticTheta(a, x, q)|
|kepler_e|&#40;-inf,+inf&#41;|6|inverse kepler's equation, e(eccentricity) &leq; 256|ddouble.KeplerE(m, e, centered)|
|agm|&#91;0,+inf&#41;|2||ddouble.Agm(a, b)|
|fresnel_c|&#40;-inf,+inf&#41;|4||ddouble.FresnelC(x)|
|fresnel_s|&#40;-inf,+inf&#41;|4||ddouble.FresnelS(x)|
|ei|&#40;-inf,+inf&#41;|4|exponential integral|ddouble.Ei(x)|
|ein|&#40;-inf,+inf&#41;|4|complementary exponential integral|ddouble.Ein(x)|
|li|&#91;0,+inf&#41;|5|logarithmic integral li(x)=ei(log(x))|ddouble.Li(x)|
|si|&#40;-inf,+inf&#41;|4|sin integral, limit_zero=true: si(x)|ddouble.Si(x, limit_zero)|
|ci|&#91;0,+inf&#41;|4|cos integral|ddouble.Ci(x)|
|shi|&#40;-inf,+inf&#41;|5|hyperbolic sin integral|ddouble.Shi(x)|
|chi|&#91;0,+inf&#41;|5|hyperbolic cos integral|ddouble.Chi(x)|
|lambert_w|&#91;-1/e,+inf&#41;|4||ddouble.LambertW(x)|
|airy_ai|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near root.|ddouble.AiryAi(x)|
|airy_bi|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near root.|ddouble.AiryBi(x)|
|jacobi_sn|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|ddouble.JacobiSn(x, m)|
|jacobi_cn|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|ddouble.JacobiCn(x, m)|
|jacobi_dn|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|ddouble.JacobiDn(x, m)|
|jacobi_amplitude|&#40;-inf,+inf&#41;|4|k: elliptic modulus, m=k^2|ddouble.JacobiAm(x, m)|
|inverse_jacobi_sn|&#91;-1,+1&#93;|4|k: elliptic modulus, m=k^2|ddouble.JacobiArcSn(x, m)|
|inverse_jacobi_cn|&#91;-1,+1&#93;|4|k: elliptic modulus, m=k^2|ddouble.JacobiArcCn(x, m)|
|inverse_jacobi_dn|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.JacobiArcDn(x, m)|
|carlson_rd|&#91;0,+inf&#41;|4||ddouble.CarlsonRD(x, y, z)|
|carlson_rc|&#91;0,+inf&#41;|4||ddouble.CarlsonRC(x, y)|
|carlson_rf|&#91;0,+inf&#41;|4||ddouble.CarlsonRF(x, y, z)|
|carlson_rj|&#91;0,+inf&#41;|4||ddouble.CarlsonRJ(x, y, z, w)|
|carlson_rg|&#91;0,+inf&#41;|4||ddouble.CarlsonRG(x, y, z)|
|riemann_zeta|&#40;-inf,+inf&#41;|3||ddouble.RiemannZeta(x)|
|hurwitz_zeta|&#40;1,+inf&#41;|3|a &geq; 0|ddouble.HurwitzZeta(x, a)|
|dirichlet_eta|&#40;-inf,+inf&#41;|3||ddouble.DirichletEta(x)|
|polylog|&#40;-inf,1&#93;|3|n &in; &#91;-4,8&#93;|ddouble.Polylog(n, x)|
|owen's_t|&#40;-inf,+inf&#41;|5||ddouble.OwenT(h, a)|
|bump|&#40;-inf,+inf&#41;|4|C-infinity smoothness basis function, bump(x)=1/(exp(1/x-1/(1-x))+1)|ddouble.Bump(x)|
|hermite_h|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.HermiteH(n, x)|
|laguerre_l|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.LaguerreL(n, x)|
|associated_laguerre_l|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.LaguerreL(n, alpha, x)|
|legendre_p|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.LegendreP(n, x)|
|associated_legendre_p|&#91;-1,1&#93;|3|n &leq; 64|ddouble.LegendreP(n, m, x)|
|chebyshev_t|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.ChebyshevT(n, x)|
|chebyshev_u|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.ChebyshevU(n, x)|
|zernike_r|&#91;0,1&#93;|3|n &leq; 64|ddouble.ZernikeR(n, m, x)|
|gegenbauer_c|&#40;-inf,+inf&#41;|3|n &leq; 64|ddouble.GegenbauerC(n, alpha, x)|
|jacobi_p|&#91;-1,1&#93;|3|n &leq; 64, alpha,beta &gt; -1|ddouble.JacobiP(n, alpha, beta, x)|
|bernoulli|&#91;0,1&#93;|4|n &leq; 64, centered: x->x-1/2|ddouble.Bernoulli(n, x, centered)|
|mathieu_eigenvalue_a|&#40;-inf,+inf&#41;|4|n &leq; 16|ddouble.MathieuA(n, q)|
|mathieu_eigenvalue_b|&#40;-inf,+inf&#41;|4|n &leq; 16|ddouble.MathieuB(n, q)|
|mathieu_ce|&#40;-inf,+inf&#41;|4|n &leq; 16, Accuracy deteriorates when q is very large.|ddouble.MathieuC(n, q, x)|
|mathieu_se|&#40;-inf,+inf&#41;|4|n &leq; 16, Accuracy deteriorates when q is very large.|ddouble.MathieuS(n, q, x)|
|ldexp|&#40;-inf,+inf&#41;|N/A||ddouble.Ldexp(x, y)|
|binomial|N/A|1|n &leq; 1000|ddouble.Binomial(n, k)|
|hypot|N/A|2||ddouble.Hypot(x, y)|
|min|N/A|N/A||ddouble.Min(x, y)|
|max|N/A|N/A||ddouble.Max(x, y)|
|clamp|N/A|N/A||ddouble.Clamp(v, min, max)|
|copysign|N/A|N/A||ddouble.CopySign(value, sign)|
|floor|N/A|N/A||ddouble.Floor(x)|
|ceiling|N/A|N/A||ddouble.Ceiling(x)|
|round|N/A|N/A||ddouble.Round(x)|
|truncate|N/A|N/A||ddouble.Truncate(x)|
|array sum|N/A|N/A||IEnumerable&lt;ddouble&gt;.Sum()|
|array average|N/A|N/A||IEnumerable&lt;ddouble&gt;.Average()|
|array min|N/A|N/A||IEnumerable&lt;ddouble&gt;.Min()|
|array max|N/A|N/A||IEnumerable&lt;ddouble&gt;.Max()|

## Constants

|constant|value|note|usage|
|----|----|----|----|
|Pi|3.141592653589793238462...||ddouble.PI|
|Napier's E|2.718281828459045235360...||ddouble.E|
|Euler's Gamma|0.577215664901532860606...||ddouble.EulerGamma|
|&zeta;(3)|1.202056903159594285399...|Apery const.|ddouble.Zeta3|
|&zeta;(5)|1.036927755143369926331...||ddouble.Zeta5|
|&zeta;(7)|1.008349277381922826839...||ddouble.Zeta7|
|&zeta;(9)|1.002008392826082214418...||ddouble.Zeta9|
|Positive root of digamma|1.461632144968362341263...||ddouble.DigammaZero|
|Erdös Borwein constant|1.606695152415291763783...||ddouble.ErdosBorwein|
|Feigenbaum constant|4.669201609102990671853...||ddouble.FeigenbaumDelta|
|Lemniscate constant|2.622057554292119810465...||ddouble.LemniscatePI|

## Sequence

|sequence|note|usage|
|----|----|----|
|Taylor|1/n!|ddouble.TaylorSequence|
|Factorial|n!|ddouble.Factorial|
|Bernoulli|B(2k)|ddouble.BernoulliSequence|
|HarmonicNumber|H_n|ddouble.HarmonicNumber|
|StieltjesGamma|&gamma;_n|ddouble.StieltjesGamma|

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
