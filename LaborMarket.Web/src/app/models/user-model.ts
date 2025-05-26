export type UserDataModel = {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
    passwordHash: string;
    createdAt: string;
};

export type CreateUserModel = Omit<UserDataModel, 'userId'>;