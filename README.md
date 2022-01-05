# DoubleDouble
 double-double arithmetic implements
 
## Requirement
.NET 5.0

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
|gamma|&#40;-inf,+inf&#41;|5|Accuracy deteriorates near non-positive intergers. <br/> If x is Natual number lass than 35, an exact integer value is returned. |ddouble.Gamma(x)|
|loggamma|&#40;0,+inf&#41;|5|Near the positive zero point, polynomial interpolation is used.|ddouble.LogGamma(x)|
|digamma|&#40;-inf,+inf&#41;|5|Near the positive zero point, polynomial interpolation is used.|ddouble.Digamma(x)|
|erf|&#40;-inf,+inf&#41;|5||ddouble.Erf(x)|
|erfc|&#40;-inf,+inf&#41;|5||ddouble.Erfc(x)|
|inverse_erf|&#40;-1,1&#41;|8||ddouble.InverseErf(x)|
|inverse_erfc|&#40;0,2&#41;|8||ddouble.InverseErfc(x)|
|bessel_j|&#91;0,+inf&#41;|16|Accuracy deteriorates near zero points.<br/>abs(nu) &leq; 8 |ddouble.BesselJ(nu, x)|
|bessel_y|&#91;0,+inf&#41;|16|Accuracy deteriorates near zero points.<br/>abs(nu) &leq; 8 |ddouble.BesselY(nu, x)|
|bessel_i|&#91;0,+inf&#41;|16|abs(nu) &leq; 8 |ddouble.BesselI(nu, x)|
|bessel_k|&#91;0,+inf&#41;|16|abs(nu) &leq; 8 |ddouble.BesselK(nu, x)|
|elliptic_k|&#91;0,1&#93;|1|k: elliptic modulus, m=k^2|ddouble.EllipticK(k)|
|elliptic_e|&#91;0,1&#93;|1|k: elliptic modulus, m=k^2|ddouble.EllipticE(k)|
|elliptic_pi|&#91;0,1&#93;|1|k: elliptic modulus, m=k^2|ddouble.EllipticPi(n, k)|
|fresnel_c|&#40;-inf,+inf&#41;|8||ddouble.FresnelC(x)|
|fresnel_s|&#40;-inf,+inf&#41;|8||ddouble.FresnelS(x)|
|ei|&#40;-inf,+inf&#41;|8|exponential integral|ddouble.Ei(x)|
|li|&#91;0,+inf&#41;|10|logarithmic integral li(x)=ei(log(x))|ddouble.Li(x)|
|lambertw|&#91;-1/e,+inf&#41;|8||ddouble.LambertW(x)|
|airy_ai|&#40;-inf,+inf&#41;|10|Accuracy deteriorates near zero points.|ddouble.AiryAi(x)|
|airy_bi|&#40;-inf,+inf&#41;|10|Accuracy deteriorates near zero points.|ddouble.AiryBi(x)|
|lower incomplete gamma|&#91;0,+inf&#41;|10|nu &leq; 32|ddouble.LowerIncompleteGamma(nu, x)|
|upper incomplete gamma|&#91;0,+inf&#41;|10|nu &leq; 32|ddouble.UpperIncompleteGamma(nu, x)|
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
