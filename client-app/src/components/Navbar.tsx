import { useAuth } from "@/providers/auth"
import { MaxWidthWrapper } from "./MaxWidthWrapper"
import { buttonVariants } from "./ui/button"
import { ArrowRight } from "lucide-react"
import { Link } from "react-router-dom"
import { ROUTE_ENUM } from "@/App"

export function Navbar() {
  const { isAuthenticated, isClient, isBarberShop, isAdmin } = useAuth()
  
  return (
    <nav className="sticky z-[100] h-14 inset-x-0 top-0 w-full border-b border-gray-200 bg-white/75 backdrop-blur-lg transition-all">
      <MaxWidthWrapper>
        <div className="flex h-14 items-center justify-between border-b border-zinc 200">
          <Link to={ROUTE_ENUM.ROOT} className="flex z-40 font-semibold">
            use<span style={{ color: 'hsl(var(--primary))'}}>case</span>
          </Link>

          <div className="h-full flex items-center space-x-4">
            {isAuthenticated ? (
              <>
                <Link to={ROUTE_ENUM.LOGOUT} className={buttonVariants({
                  size: 'sm',
                  variant: 'ghost',
                })}>
                  Logout
                </Link>
                {isClient && (
                  <Link to={ROUTE_ENUM.ADMIN} className={buttonVariants({
                    size: 'sm',
                    variant: 'ghost',
                  })}>
                    Dashboard ✨
                  </Link>
                )}
                {isBarberShop && (
                  <Link to={ROUTE_ENUM.ADMIN} className={buttonVariants({
                    size: 'sm',
                    variant: 'ghost',
                  })}>
                    Dashboard ✨
                  </Link>
                )}
                {isAdmin && (
                  <Link to={ROUTE_ENUM.ADMIN} className={buttonVariants({
                    size: 'sm',
                    variant: 'ghost',
                  })}>
                    Dashboard ✨
                  </Link>
                )}
                <Link to="/configure/upload" className={buttonVariants({
                  size: 'sm',
                  className: 'hidden sm:flex items-center gap-1',
                })}>
                  Create case
                  <ArrowRight className="ml-1.5 h-5 w-5" />
                </Link>
              </>
            ) : (
              <>
                <Link to={ROUTE_ENUM.REGISTER} className={buttonVariants({
                  size: 'sm',
                  variant: 'ghost',
                })}>
                  Cadastrar
                </Link>
                <Link to={ROUTE_ENUM.LOGIN} className={buttonVariants({
                  size: 'sm',
                  variant: 'ghost',
                })}>
                  Login
                </Link>

                <div className="h-8 w-px bg-zinc-200 hidden sm:block" />

                <Link to="/configure/upload" className={buttonVariants({
                  size: 'sm',
                  className: 'hidden sm:flex items-center gap-1',
                })}>
                  Create case
                  <ArrowRight className="ml-1.5 h-5 w-5" />
                </Link>
              </>
            )}
          </div>
        </div>
      </MaxWidthWrapper>
    </nav>
  )
}