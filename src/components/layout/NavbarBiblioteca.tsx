import Link from "next/link";
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuList,
  NavigationMenuLink,
} from "@/components/ui/navigation-menu";

export function NavbarBiblioteca() {
  return (
    <nav className="w-full flex justify-center bg-white border-b mb-8">
      <NavigationMenu>
        <NavigationMenuList>
          <NavigationMenuItem>
            <Link href="/" legacyBehavior passHref>
              <NavigationMenuLink className="px-4 py-2 font-medium hover:text-blue-600">
                Login/Perfil
              </NavigationMenuLink>
            </Link>
          </NavigationMenuItem>
          <NavigationMenuItem>
            <Link href="/aluno" legacyBehavior passHref>
              <NavigationMenuLink className="px-4 py-2 font-medium hover:text-blue-600">
                Aluno
              </NavigationMenuLink>
            </Link>
          </NavigationMenuItem>
          <NavigationMenuItem>
            <Link href="/professor" legacyBehavior passHref>
              <NavigationMenuLink className="px-4 py-2 font-medium hover:text-blue-600">
                Professor
              </NavigationMenuLink>
            </Link>
          </NavigationMenuItem>
          <NavigationMenuItem>
            <Link href="/bibliotecario" legacyBehavior passHref>
              <NavigationMenuLink className="px-4 py-2 font-medium hover:text-blue-600">
                Bibliotecário
              </NavigationMenuLink>
            </Link>
          </NavigationMenuItem>
        </NavigationMenuList>
      </NavigationMenu>
    </nav>
  );
}
