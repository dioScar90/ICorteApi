// import Link from "next/link"
import { MaxWidthWrapper } from "./MaxWidthWrapper"
import { buttonVariants } from "./ui/button"
import { ArrowRight } from "lucide-react"
// import { getKindeServerSession } from "@kinde-oss/kinde-auth-nextjs/server"
import { Link } from "react-router-dom"

export function Navbar() {
  // const { getUser } = getKindeServerSession()
  // const user = await getUser()
  // const isAdmin = user?.email === process.env.ADMIN_EMAIL
  const user = { email: 'diogols@live.com' }
  const isAdmin = user?.email === 'diogols@live.com'

  return (
    <nav className="sticky z-[100] h-14 inset-x-0 top-0 w-full border-b border-gray-200 bg-white/75 backdrop-blur-lg transition-all">
      <MaxWidthWrapper>
        <div className="flex h-14 items-center justify-between border-b border-zinc 200">
          <Link to="/" className="flex z-40 font-semibold">
            use<span style={{ color: 'hsl(var(--primary))'}}>case</span>
          </Link>

          <div className="h-full flex items-center space-x-4">
            {user ? (
              <>
                <Link to="/api/auth/logout" className={buttonVariants({
                  size: 'sm',
                  variant: 'ghost',
                })}>
                  Sign out
                </Link>
                {isAdmin && (
                  <Link to="/dashboard" className={buttonVariants({
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
                <Link to="/api/auth/register" className={buttonVariants({
                  size: 'sm',
                  variant: 'ghost',
                })}>
                  Sign up
                </Link>
                <Link to="/api/auth/login" className={buttonVariants({
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