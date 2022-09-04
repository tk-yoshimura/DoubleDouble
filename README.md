# DoubleDouble
 double-double arithmetic implements 
 
## Requirement
.NET 6.0

## Install

[Download DLL](https://github.com/tk-yoshimura/DoubleDouble/releases)  
[Download Nuget](https://www.nuget.org/packages/tyoshimura.doubledouble/)  

- To install, just import the DLL.
- This library does not change the environment at all.

## More Precision ?
[MultiPrecision](https://github.com/tk-yoshimura/MultiPrecision)  

## Types

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
|log2|&#40;0,+inf&#41;|2||ddouble.Log2(x)|
|log|&#40;0,+inf&#41;|3||ddouble.Log(x)|
|log10|&#40;0,+inf&#41;|3||ddouble.Log10(x)|
|log1p|&#40;-1,+inf&#41;|3|log(1+x)|ddouble.Log1p(x)|
|pow2|&#40;-inf,+inf&#41;|1||ddouble.Pow2(x)|
|pow|&#40;-inf,+inf&#41;|4||ddouble.Pow(x, y)|
|pow10|&#40;-inf,+inf&#41;|4||ddouble.Pow10(x)|
|exp|&#40;-inf,+inf&#41;|4||ddouble.Exp(x)|
|expm1|&#40;-inf,+inf&#41;|4|exp(x)-1|ddouble.Expm1(x)|
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
|gamma|&#40;-inf,+inf&#41;|4|Accuracy deteriorates near non-positive intergers. <br/> If x is Natual number lass than 35, an exact integer value is returned. |ddouble.Gamma(x)|
|loggamma|&#40;0,+inf&#41;|5||ddouble.LogGamma(x)|
|digamma|&#40;-inf,+inf&#41;|5|Near the positive zero point, polynomial interpolation is used.|ddouble.Digamma(x)|
|polygamma|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near non-positive intergers. <br/> n &leq; 16|ddouble.Polygamma(n, x)|
|beta|&#91;0,+inf&#41;|5||ddouble.Beta(a, b)|
|incomplete_beta|&#91;0,1&#93;|8|Accuracy decreases when the radio of a,b is too large. a,b &leq; 64|ddouble.IncompleteBeta(x, a, b)|
|erf|&#40;-inf,+inf&#41;|5||ddouble.Erf(x)|
|erfc|&#40;-inf,+inf&#41;|5||ddouble.Erfc(x)|
|inverse_erf|&#40;-1,1&#41;|8||ddouble.InverseErf(x)|
|inverse_erfc|&#40;0,2&#41;|8||ddouble.InverseErfc(x)|
|erfi|&#40;-inf,+inf&#41;|8||ddouble.Erfi(x)|
|dawson_f|&#40;-inf,+inf&#41;|4||ddouble.DawsonF(x)|
|bessel_j|&#91;0,+inf&#41;|16|Accuracy deteriorates near zero points.<br/>abs(nu) &leq; 8 |ddouble.BesselJ(nu, x)|
|bessel_y|&#91;0,+inf&#41;|16|Accuracy deteriorates near zero points.<br/>abs(nu) &leq; 8 |ddouble.BesselY(nu, x)|
|bessel_i|&#91;0,+inf&#41;|16|abs(nu) &leq; 8 |ddouble.BesselI(nu, x)|
|bessel_k|&#91;0,+inf&#41;|16|abs(nu) &leq; 8 |ddouble.BesselK(nu, x)|
|struve_h|&#40;-inf,+inf&#41;|16|0 &leq; n &leq; 8|ddouble.StruveH(n, x)|
|struve_k|&#91;0,+inf&#41;|16|0 &leq; n &leq; 8|ddouble.StruveK(n, x)|
|struve_l|&#40;-inf,+inf&#41;|16|0 &leq; n &leq; 8|ddouble.StruveL(n, x)|
|struve_m|&#91;0,+inf&#41;|16|0 &leq; n &leq; 8|ddouble.StruveM(n, x)|
|elliptic_k|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticK(m)|
|elliptic_e|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticE(m)|
|elliptic_pi|&#91;0,1&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticPi(n, m)|
|incomplete_elliptic_k|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticK(x, m)|
|incomplete_elliptic_e|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2|ddouble.EllipticE(x, m)|
|incomplete_elliptic_pi|&#91;0,2pi&#93;|4|k: elliptic modulus, m=k^2<br/>Argument order follows wolfram.|ddouble.EllipticPi(n, x, m)|
|elliptic_theta1|&#40;-inf,+inf&#41;|4|q &leq; 0.995|ddouble.EllipticTheta1(x, q)|
|elliptic_theta2|&#40;-inf,+inf&#41;|4|q &leq; 0.995|ddouble.EllipticTheta2(x, q)|
|elliptic_theta3|&#40;-inf,+inf&#41;|4|q &leq; 0.995|ddouble.EllipticTheta3(x, q)|
|elliptic_theta4|&#40;-inf,+inf&#41;|4|q &leq; 0.995|ddouble.EllipticTheta4(x, q)|
|agm|&#91;0,+inf&#41;|2||ddouble.Agm(a, b)|
|fresnel_c|&#40;-inf,+inf&#41;|8||ddouble.FresnelC(x)|
|fresnel_s|&#40;-inf,+inf&#41;|8||ddouble.FresnelS(x)|
|ei|&#40;-inf,+inf&#41;|8|exponential integral|ddouble.Ei(x)|
|ein|&#40;-inf,+inf&#41;|8|complementary exponential integral|ddouble.Ein(x)|
|li|&#91;0,+inf&#41;|10|logarithmic integral li(x)=ei(log(x))|ddouble.Li(x)|
|si|&#40;-inf,+inf&#41;|8|sin integral, limit_zero=true: si(x)|ddouble.Si(x, limit_zero)|
|ci|&#91;0,+inf&#41;|8|cos integral|ddouble.Ci(x)|
|shi|&#40;-inf,+inf&#41;|8|hyperbolic sin integral|ddouble.Shi(x)|
|chi|&#91;0,+inf&#41;|8|hyperbolic cos integral|ddouble.Chi(x)|
|lambertw|&#91;-1/e,+inf&#41;|8||ddouble.LambertW(x)|
|airy_ai|&#40;-inf,+inf&#41;|10|Accuracy deteriorates near zero points.|ddouble.AiryAi(x)|
|airy_bi|&#40;-inf,+inf&#41;|10|Accuracy deteriorates near zero points.|ddouble.AiryBi(x)|
|lower_incomplete_gamma|&#91;0,+inf&#41;|10|nu &leq; 128|ddouble.LowerIncompleteGamma(nu, x)|
|upper_incomplete_gamma|&#91;0,+inf&#41;|10|nu &leq; 128|ddouble.UpperIncompleteGamma(nu, x)|
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
|dirichlet_eta|&#40;-inf,+inf&#41;|3||ddouble.DirichletEta(x)|
|polylog|&#40;-inf,1&#93;|3|n &in; &#91;-4,8&#93;|ddouble.Polylog(n, x)|
|owen's_t|&#40;-inf,+inf&#41;|10||ddouble.OwenT(h, a)|
|ldexp|&#40;-inf,+inf&#41;|N/A||ddouble.Ldexp(x, y)|
|min|N/A|N/A||ddouble.Min(x, y)|
|max|N/A|N/A||ddouble.Max(x, y)|
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

## Sequence

|sequence|note|usage|
|----|----|----|
|Taylor|1/n!|ddouble.TaylorSequence|
|Factorial|n!|ddouble.Factorial|
|Bernoulli|B(2k)|ddouble.BernoulliSequence|
|HarmonicNumber|H_n|ddouble.HarmonicNumber|

## Casts

- long (accurately)

  ddouble v0 = 123;

  long n0 = (long)v0;

- double (accurately)

  ddouble v1 = 0.5;

  double n1 = (double)v1;
  
- decimal (approximately)

  ddouble v1 = 0.1m;

  decimal n1 = (decimal)v1;

- string (approximately)

  ddouble v2 = "3.14e0";

  string s0 = v2.ToString();

  string s1 = v2.ToString("E8");

  string s2 = $"{v2:E8}";
  
## I/O

BinaryWriter, BinaryReader

## Licence
[MIT](https://github.com/tk-yoshimura/DoubleDouble/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)
