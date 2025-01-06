/** @type {import('tailwindcss').Config} */
export default {
  content: [
    './index.html', // Root index.html
    './src/**/*.{vue,js,ts,jsx,tsx}', // Vue, JS, TS files in the src folder
    './node_modules/primevue/**/*.js', // Include PrimeVue components
    './node_modules/primeicons/**/*.js', // Include PrimeIcons
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};