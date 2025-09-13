import axiosInstance from "../api/axiosInstance";
import type { RegisterDto, LoginDto } from '../types/auth';

export const authService = {
  register: async (data: RegisterDto) => {
    console.log("sending register call to api: ", data);
    const res = await axiosInstance.post("/auth/register", data);
    console.log("got response: ", res);
    return res.data;
  },
  login: async (data: LoginDto) => {
    console.log("sending login call to api: ", data);
    const res = await axiosInstance.post("/auth/login", data);
    console.log("got response: ", res);
    return res.data; // expected: { token: string }
  },
};