import { Outlet } from "react-router-dom";
import { Footer } from "../Footer";
// import { Header } from "./Header";
import "@/index.css"
import { Toaster } from "../ui/toaster";
import Providers from "../Providers";
import { Navbar } from "../Navbar";

export function BaseLayout() {
  return (
    <>
      {/* <Header /> */}
      <Navbar />
      
      <main className="flex grainy-light flex-col min-h-[calc(100vh-3.5rem-1px)]">
          <div className="flex-1 flex flex-col h-full">
            <Providers>
              <Outlet />
            </Providers>
          </div>

          <Footer />
        </main>

      <Toaster />
    </>
  )
}
