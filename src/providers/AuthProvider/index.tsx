import { AuthContext } from "../../contexts/auth/AuthContext";
import { useAuthProviderViewModel } from "./useAuthProviderViewModel";
import React, { FC } from 'react';

interface IAuthProviderProps {
  children: React.ReactNode
}

export const AuthProvider: FC<IAuthProviderProps> = ({ children }) => {
  const { user, isLoading, register, login, loginWithGoogle, logout } = useAuthProviderViewModel();

  return (
    <AuthContext.Provider
      value={{ user, isLoading, register, login, loginWithGoogle, logout }}
    >
      {!isLoading && children}
    </AuthContext.Provider>
  );
};
