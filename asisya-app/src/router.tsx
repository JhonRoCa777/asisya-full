export const ROUTER = {
  MAIN: '',
  HOME: {
    MAIN: 'home'
  },
  LOGIN: () => window.location.href = import.meta.env.VITE_APP_URL
};