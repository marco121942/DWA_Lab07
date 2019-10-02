using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    class Program
    {
        public static DataClassesDataContext context = new DataClassesDataContext();

        static void Main(string[] args)
        {
            var Pedidos = context.Pedidos.ToList();
            var Clientes = context.clientes.ToList();
            var DetallePedidos = context.detallesdepedidos.ToList();

   
   
            var Productos = context.productos.ToList();
            var Proveedores = context.proveedores.ToList();

            //AmmountGreaterThan(Pedidos, DetallePedidos);
            // LastFiveYears(Pedidos);
            //MoreThanTwo(Pedidos);
            //Proveedores2Products(Productos, Proveedores);
            Productos3Pedidos(Pedidos, DetallePedidos, Productos);
            Console.Read();
        }

        static void IntroToLINQ()
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;

            foreach (int num in numQuery)
            {
                Console.WriteLine("{0,1}", num);
            }
        }

        static void DataSource(List<clientes> clientes)
        {
            var queryAllCustomers = from cust in clientes
                                    select cust;

            foreach (var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       select cust;
            foreach (var item in queryLondonCustomers)
            {
                Console.WriteLine(item.NombreCompañia + " - " + item.Ciudad);
            }
        }

        static void Ordering()
        {
            var queryLondonCustomers3 =
                from cust in context.clientes
                where cust.Ciudad == "Londres"
                orderby cust.NombreCompañia ascending
                select cust;

            foreach (var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Grouping()
        {
            var queryCustomersByCity =
                from cust in context.clientes
                group cust by cust.Ciudad;

            foreach (var customerGroup in queryCustomersByCity)
            {
                Console.WriteLine(customerGroup.Key);
                foreach (clientes customer in customerGroup)
                {
                    Console.WriteLine("    {0}", customer.NombreCompañia);
                }
            }
        }

        static void Grouping2()
        {
            var custQuery =
                from cust in context.clientes
                group cust by cust.Ciudad into custGroup
                where custGroup.Count() > 2
                orderby custGroup.Key
                select custGroup;

            foreach (var item in custQuery)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void Joining()
        {
            var innerJoinQuery =
                from cust in context.clientes
                join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };

            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        //1
        static void LastFiveYears(List<Pedidos> pedidos)
        {
            var lastFiveYears =
                from ped in pedidos
                where Convert.ToDateTime(ped.FechaPedido).Year > Convert.ToDateTime("05/05/1995").Year
                select ped;

            foreach (var item in lastFiveYears)
            {
                Console.WriteLine(item.IdPedido + " " + item.Destinatario);
            }
        }

        //2
        static void MoreThanTwo(List<Pedidos> pedidos)
        {
            var moreThanTwo =
                from ped in pedidos
                group ped by ped.IdCliente into grp
                where grp.Count() > 2
                orderby grp.Key
                select grp;

            foreach (var item in moreThanTwo)
            {
                Console.WriteLine(item.Key);
            }
        }

        //3
        static void AmmountGreaterThan(List<Pedidos> Pedidos, List<detallesdepedidos> detPedidos)
        {
   
            var monto = detPedidos.GroupBy(l => l.idpedido)
                      .Select(lg =>
                            new {
                                Owner = lg.Key,
                                Boxes = lg.Count(),
                                monto = lg.Sum(w => (w.preciounidad * w.cantidad))
                            });
            foreach (var item in monto)
            {
                Console.WriteLine(item.monto);
            }
        }
        //4
        static void Proveedores2Products(List<productos> productos, List<proveedores> proveedores)
        {

            var lista = proveedores.Where(x => x.productos.Count() > 2);

            foreach (var item in lista)
            {
                Console.WriteLine(item.nombreCompañia);
            }
        }
        //5
        static void Productos3Pedidos(List<Pedidos> Pedidos, List<detallesdepedidos> detalledepedidos, List<productos> Productos)
        {

            var lista = from ped in Pedidos
                        join det in detalledepedidos on ped.IdPedido equals det.idpedido
                        join pro in Productos on det.idproducto equals pro.idproducto
                        where det.idpedido > 3
                        select new { ProductoName = pro.nombreProducto };

            foreach (var item in lista)
            {
                Console.WriteLine(item.ProductoName);
            }

            var providers = Productos.GroupBy(x => x.proveedores.nombreCompañia)
                .Where(x => x.Count() >= 2).Select(item => new { Proveedor = item.Key, Count = item.Count() });

            foreach (var item in providers)
            {
                Console.WriteLine(item);
            }
        }

        //6
        //7

    }
}
