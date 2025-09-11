import axiosInstance from "./axiosInstance";

export interface RegisterDto {
  username: string;
  password: string;
}

export interface LoginDto {
  username: string;
  password: string;
}

export const authService = {
  register: async (data: RegisterDto) => {
    const res = await axiosInstance.post("/auth/register", data);
    return res.data;
  },
  login: async (data: LoginDto) => {
    const res = await axiosInstance.post("/auth/login", data);
    return res.data; // expected: { token: string }
  },
};