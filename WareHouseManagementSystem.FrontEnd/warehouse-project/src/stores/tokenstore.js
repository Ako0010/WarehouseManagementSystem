import {create} from "zustand"
import {persist} from "zustand/middleware"

export const useTokens = create(persist(
  (set) => ({
  accessToken: "",
  refreshToken: "",
  role: null,
  setTokens: (accessToken, refreshToken,role) => set({ accessToken, refreshToken,role }),
  setAccessToken: (token) => set((state) => ({...state, accessToken: token })),
  setRefreshToken: (token) => set((state) => ({...state, refreshToken: token })),
  clearTokens: () => set((state) => ({...state, accessToken: "",refreshToken: "" ,role: null})),
  }),{name: "tokens"}
));