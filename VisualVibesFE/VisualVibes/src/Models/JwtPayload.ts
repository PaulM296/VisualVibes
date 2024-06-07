export interface JwtPayload {
    sub: string;
    email: string;
    userId: string;
    role: string;
    nbf: number;
    exp: number;
    iat: number;
    iss: string;
    aud: string;
  }
  