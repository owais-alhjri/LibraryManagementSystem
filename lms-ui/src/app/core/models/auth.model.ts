export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface AuthResponse{
  token: string;
}

export interface TokenPayload{
  sub: string;
  email: string;
  role: string;
  exp: number;
}
