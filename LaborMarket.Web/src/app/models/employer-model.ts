export type EmployerDataModel = {
  employerId: number;
  companyName: string;
  contactEmail: string;
  contactPhone: string;
  description: string;
  profileImageUrl: string;
};

export type CreateEmployerModel = {
  companyName: string;
  contactEmail: string;
  contactPhone: string;
  password: string;
  role: string;
};
