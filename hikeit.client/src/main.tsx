import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { Provider } from "@/components/ui/provider";
import { BrowserRouter, Routes, Route } from "react-router";
import "./index.css";
import App from "./App.tsx";
import TripsPage from "./pages/tripsPage";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Provider>
        <Routes>
          <Route path="/" element={<App />} />
          <Route path="/trips" element={<TripsPage />} />
        </Routes>
      </Provider>
    </BrowserRouter>
  </StrictMode>
);
