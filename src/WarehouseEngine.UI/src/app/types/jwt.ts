export type JwtTokenResponseOk = {
  type: 'Success';
  jwt: string;
};

export type JwtTokenResponseUnauthorized = {
  type: 'Unauthorized';
};

export type JwtTokenResponseError = {
  type: 'Failed';
  error: string;
};

export type JwtTokenResponse = Readonly<JwtTokenResponseOk | JwtTokenResponseUnauthorized | JwtTokenResponseError>;
