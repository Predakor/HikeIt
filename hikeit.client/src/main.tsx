import { createRoot } from "react-dom/client";
import { Suspense, lazy } from "react";
import { GiMountaintop } from "react-icons/gi";
import "./index.css";

const App = lazy(() => import("./App"));

createRoot(document.getElementById("root")!).render(
  <Suspense fallback={<SplashScreen />}>
    <App />
  </Suspense>
);

function SplashScreen() {
  return (
    <div id="splash">
      <GiMountaintop size={128} />
      <h1>
        Hike <span>It</span>
      </h1>
    </div>
  );
}
