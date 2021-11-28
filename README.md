# DoubleDouble
 double-double arithmetic implements 
 
## Requirement
.NET 5.0

## Install

[Download DLL](https://github.com/tk-yoshimura/DoubleDouble/releases)  

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
|ldexp|&#40;-inf,+inf&#41;|N/A||ddouble.Ldexp(x, y)|
|min|N/A|N/A||ddouble.Min(x, y)|
|max|N/A|N/A||ddouble.Max(x, y)|
|floor|N/A|N/A||ddouble.Floor(x)|
|ceiling|N/A|N/A||ddouble.Ceiling(x)|
|round|N/A|N/A||ddouble.Round(x)|
|truncate|N/A|N/A||ddouble.Truncate(x)|
|array sum|N/A|N/A|kahan summation|IEnumerable&lt;ddouble&gt;.Sum()|
|array average|N/A|N/A|kahan summation|IEnumerable&lt;ddouble&gt;.Average()|
|array min|N/A|N/A||IEnumerable&lt;ddouble&gt;.Min()|
|array max|N/A|N/A||IEnumerable&lt;ddouble&gt;.Max()|

## Constants

|constant|value|note|usage|
|----|----|----|----|
|Pi|3.141592653589793238462...||ddouble.PI|
|Napier's E|2.718281828459045235360...||ddouble.E|

## Sequence

|sequence|note|usage|
|----|----|----|
|Taylor|1/n!|ddouble.TaylorSequence|

## Coefficient

|coefficient|note|usage|
|----|----|----|

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
