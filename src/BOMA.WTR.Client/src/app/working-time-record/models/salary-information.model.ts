export interface SalaryInformationModel {
	// Base rate
	baseSalary: number;
	base50PercentageSalary: number;
	base100PercentageSalary: number;
	baseSaturdaySalary: number;

	// Gross
	grossBaseSalary: number;
	grossBase50PercentageSalary: number;
	grossBase100PercentageSalary: number;
	grossBaseSaturdaySalary: number;

	// Bonus
	bonusBaseSalary: number;
	bonusBase50PercentageSalary: number;
	bonusBase100PercentageSalary: number;
	bonusBaseSaturdaySalary: number;
	bonusFromGrossSumSalaryCustomPercentageSalary: number;

	// Gross Sum
	grossSumBaseSalary: number;
	grossSumBase50PercentageSalary: number;
	grossSumBase100PercentageSalary: number;
	grossSumBaseSaturdaySalary: number;

	// Bonus Sum
	bonusSumSalary: number;

	// Night
	nightBaseSalary: number;
	nightWorkedHours: number;

	// Additional
	holidaySalary: number;
	sicknessSalary: number;
	additionalSalary: number;

	finalSumSalary: number;
}
