export type UserDataModel = {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  passwordHash: string;
  createdAt: string;
  cv: string;
  profileImageUrl?: string;
};

export type CreateUserModel = {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  createdAt: string;
  role: string;
};
