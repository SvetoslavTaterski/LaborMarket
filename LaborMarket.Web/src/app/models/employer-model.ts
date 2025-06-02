export type EmployerDataModel = {
  employerId: number;
  companyName: string;
  contactEmail: string;
  contactPhone: string;
};

export type CreateEmployerModel = {
  companyName: string;
  contactEmail: string;
  contactPhone: string;
  companyPassword: string;
  role: string;
};
