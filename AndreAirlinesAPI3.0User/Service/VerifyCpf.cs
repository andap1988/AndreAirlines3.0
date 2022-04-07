namespace AndreAirlinesAPI3._0User.Service
{
    public class VerifyCpf
    {
		public static bool IsValidCpf(string cpf)
		{
			int[] multiply1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiply2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digit;
			int sum;
			int rest;

			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != 11)
				return false;

			tempCpf = cpf.Substring(0, 9);
			sum = 0;

			for (int i = 0; i < 9; i++)
				sum += int.Parse(tempCpf[i].ToString()) * multiply1[i];

			rest = sum % 11;

			if (rest < 2)
				rest = 0;
			else
				rest = 11 - rest;

			digit = rest.ToString();

			tempCpf = tempCpf + digit;

			sum = 0;

			for (int i = 0; i < 10; i++)
				sum += int.Parse(tempCpf[i].ToString()) * multiply2[i];

			rest = sum % 11;

			if (rest < 2)
				rest = 0;
			else
				rest = 11 - rest;

			digit = digit + rest.ToString();

			return cpf.EndsWith(digit);
		}
	}
}
