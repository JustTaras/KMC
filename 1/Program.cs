const int n = 76;
const double fraction = 0.6;
const int firstNumber = -40;
const int secondNumber = -20;
const float ieeeNumber = 0.5f;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Практичне заняття No1: системи числення і машинне представлення даних");
Console.WriteLine("Варіант No5\n");

Console.WriteLine("1. Ручні розрахунки та перевірка конвертерами");
Console.WriteLine($"N = {n}");
foreach (int baseValue in new[] { 2, 8, 16 })
{
	Console.WriteLine($"  Основа {baseValue}: {ConvertInteger(n, baseValue)}");
}

Console.WriteLine($"F = {fraction}");
Console.WriteLine($"  Двійкові розряди: {ConvertFraction(fraction, 2, 6)}");

Console.WriteLine("\nПроміжні остачі для N = 76:");
PrintIntegerSteps(n, 2);
PrintIntegerSteps(n, 8);
PrintIntegerSteps(n, 16);

Console.WriteLine("Проміжні добутки для F = 0.6 (основа 2):");
PrintFractionSteps(fraction, 2, 6);

Console.WriteLine("\n2. 8-бітний додатковий код");
int firstBits = ToTwosComplement(firstNumber);
int secondBits = ToTwosComplement(secondNumber);
int resultBits = (firstBits + secondBits) & 0xFF;
int result = FromTwosComplement(resultBits);
bool overflow = (firstNumber < 0 && secondNumber < 0 && result >= 0)
	|| (firstNumber >= 0 && secondNumber >= 0 && result < 0);

Console.WriteLine($"  {firstNumber,3} = {ToBinary((uint)firstBits, 8)}");
Console.WriteLine($"  {secondNumber,3} = {ToBinary((uint)secondBits, 8)}");
Console.WriteLine($"  Сума бітів:       {ToBinary((uint)resultBits, 8)}");
Console.WriteLine($"  Десяткова сума: {result} (математично {firstNumber + secondNumber})");
Console.WriteLine($"  Переповнення: {overflow}");

Console.WriteLine("\n3. Розбір IEEE 754 single");
int floatBits = BitConverter.SingleToInt32Bits(ieeeNumber);
string floatBinary = ToBinary(unchecked((uint)floatBits), 32);
string signBits = floatBinary[..1];
string exponentBits = floatBinary.Substring(1, 8);
string mantissaBits = floatBinary[9..];
int exponent = Convert.ToInt32(exponentBits, 2);
int unbiasedExponent = exponent - 127;
double significand = 1 + Convert.ToInt32(mantissaBits, 2) / Math.Pow(2, 23);
float restored = BitConverter.Int32BitsToSingle(floatBits);

Console.WriteLine($"  C = {ieeeNumber}");
Console.WriteLine($"  Біти: {signBits} {exponentBits} {mantissaBits}");
Console.WriteLine($"  Знак: {signBits} ({(signBits == "0" ? "+" : "-")})");
Console.WriteLine($"  Нормалізація: 1.{mantissaBits} x 2^{unbiasedExponent}");
Console.WriteLine($"  Порядок: {exponent} (зі зміщенням 127)");
Console.WriteLine($"  Відновлене значення: {restored}");

Console.WriteLine("\n4. Похибка представлення дробів");
double left = 0.1;
double right = 0.2;
double expected = 0.3;
double actual = left + right;
Console.WriteLine($"  0.1 + 0.2 = {actual:R}");
Console.WriteLine($"  Очікуване 0.3, рівність: {actual == expected}");
Console.WriteLine("  Причина: 0.1, 0.2 і 0.3 не мають скінченного двійкового запису та зберігаються з округленням.");

static string ConvertInteger(int value, int baseValue)
{
	if (baseValue is < 2 or > 16)
	{
		throw new ArgumentOutOfRangeException(nameof(baseValue));
	}

	const string digits = "0123456789ABCDEF";
	if (value == 0)
	{
		return "0";
	}

	bool negative = value < 0;
	uint remainderValue = (uint)Math.Abs((long)value);
	string result = string.Empty;
	while (remainderValue > 0)
	{
		result = digits[(int)(remainderValue % (uint)baseValue)] + result;
		remainderValue /= (uint)baseValue;
	}

	return negative ? "-" + result : result;
}

static string ConvertFraction(double value, int baseValue, int places)
{
	if (value is < 0 or >= 1 || baseValue is < 2 or > 16 || places < 1)
	{
		throw new ArgumentOutOfRangeException();
	}

	const string digits = "0123456789ABCDEF";
	string result = string.Empty;
	for (int index = 0; index < places; index++)
	{
		value *= baseValue;
		int digit = (int)value;
		result += digits[digit];
		value -= digit;
	}

	return "0." + result;
}

static void PrintIntegerSteps(int value, int baseValue)
{
	int current = value;
	string steps = string.Empty;
	while (current > 0)
	{
		steps += $"{current} / {baseValue} = {current / baseValue}, остача {current % baseValue}; ";
		current /= baseValue;
	}

	Console.WriteLine($"  Основа {baseValue}: {steps}");
}

static void PrintFractionSteps(double value, int baseValue, int places)
{
	for (int index = 0; index < places; index++)
	{
		double product = value * baseValue;
		int digit = (int)product;
		Console.WriteLine($"  {value:R} x {baseValue} = {product:R}, цифра {digit}");
		value = product - digit;
	}
}

static int ToTwosComplement(int value)
{
	return value & 0xFF;
}

static int FromTwosComplement(int bits)
{
	return (bits & 0x80) == 0 ? bits : bits - 256;
}

static string ToBinary(uint value, int width)
{
	return Convert.ToString(value, 2).PadLeft(width, '0');
}
