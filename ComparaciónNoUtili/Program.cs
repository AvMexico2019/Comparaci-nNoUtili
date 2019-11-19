using System;
using System.Linq;
using System.Configuration;
using System.IO;
using INDAABIN_Utilerias;
using System.Data.SqlTypes;
using System.Data.Entity;
using System.Data.Objects.DataClasses;

namespace ComparaciónNoUtili
{
    public class Program
    {
        static void comparacionNoUtili()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var tablaTest = ctx.NoUtiliAuditoria2019T;
            var tablaProd = ctx.NoUtiliAuditoria2019TProduccion;
            foreach (var test in tablaTest)
            {
                Console.WriteLine("/" + test.FolioContratoArrto + "/");
                var result = (from prod in tablaProd
                              where test.FolioContratoArrto == prod.FolioContratoArrto
                              select new
                              {
                                  FolioContrato = test.FolioContratoArrto,
                                  IDInmueble = test.IdInmuebleArrendamiento,
                                  IDInmuebleProd = prod.IdInmuebleArrendamiento,
                                  Inst = test.Fk_IdInstitucion,
                                  InstProd = prod.Fk_IdInstitucion,
                                  FechaFin = test.FechaFinOcupacion,
                                  FechaFinProd = prod.FechaFinOcupacion
                              });
                foreach (var r in result)
                {
                    DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                    new DateTime(1990, 1, 1);
                    DateTime fechaProd = r.FechaFinProd != null ? (DateTime)r.FechaFinProd :
                    new DateTime(1990, 1, 1);
                    Console.WriteLine("-" + r.IDInmueble + "-" + r.IDInmuebleProd + "-" +
                        r.Inst + "-" + r.InstProd + "-" +
                        fecha + "-" + fechaProd);
                }
            }
        }

        static void BusquedaVigentes()
        {
            int NoUtiliRegsModified = 0;
            int ContratosSinInmueble = 0;
            //int registrosIrreparables = 0;
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var NoUtili = ctx.NoUtiliAuditoria2019T;
            var inmuebles = ctx.InmuebleArrendamiento;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";
            string LOGqqsaved = @"D:\temp\LOGqqsaved.txt";

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            if (File.Exists(LOGqqsaved)) File.Delete(LOGqqsaved);
            StreamWriter LOGFile = new StreamWriter(LOGqqsaved);

            // DbContextTransaction trans = ctx.Database.BeginTransaction();

            // ctx.Database.Log = LOGFile.Write; // produce demasiado texto

            //ctx.NoUtiliAuditoria2019P();
            LOGFile.WriteLine("Regeneramos la tabla NoUtili");

            LOGFile.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);
            
            try
            {
                foreach (var vigente in Vigentes)
                {
                    // localizamos el inmueble del contrato vigente via la relacion inversa y la condición es:
                    // que el IDInmueble de contrao sea igual al de la relación inversa y que
                    // el folio del contrato vigente sea igual al folio del contrato (de la tabla de contratos)
                    // de esta forma estamos probando la relación inversa
                    // result contiene los contratos vigentes que estan en la base cuyo inmueble lo podemos encontrar
                    // aun es necesaro verificar que los datos del inmueble sean corretos
                    var result = (from contrato in Contratos
                                  join relacionInv in NoUtili
                                  on contrato.Fk_IdInmuebleArrendamiento equals relacionInv.IdInmuebleArrendamiento
                                  where vigente.FolioContrato == contrato.FolioContratoArrto
                                  select new
                                  {
                                      FolioContrato = contrato.FolioContratoArrto,
                                      IdContrato = contrato.IdContratoArrto,
                                      IdInmueble = contrato.Fk_IdInmuebleArrendamiento,
                                      Inst = contrato.Fk_IdInstitucion,
                                      FechaFin = contrato.FechaFinOcupacion,
                                      FechaFinVigente = vigente.FechaContratoHasta,
                                      IdNoUtili = relacionInv.Identificador
                                  });
                    switch ((int)vigente.FolioContrato)
                    {
                        case 44360:
                        case 42058:
                        case 44421:
                        case 40014:
                        case 40015:
                            LOGFile.WriteLine("Contrato Vigente Corregido " + vigente.FolioContrato);
                            break;
                        default:
                            break;
                    }
                    int numeroRegistros = result.Count();
                    if (numeroRegistros > 0)
                    {
#if MOSTRARBUENOS
                        LOGFile.WriteLine("C /" + vigente.FolioContrato + "/ " + numeroRegistros);
                        foreach (var r in result)
                        {
                            DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                            new DateTime(1990, 1, 1);
                            DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                            new DateTime(1990, 1, 1);
                            LOGFile.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                                r.IdInmueble + "-" +
                                r.Inst + "-" +
                                fecha + "-" + fechaVigente);
                        }
#endif
                    }
                    else
                    {
                        // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
                        // porque el folio del vigente no esta en la tabla NOUTILI, se verifico que los contratos vigentes todos tienen folio Contrato
                        // y que la tabla de inmuebles todos tiene IdInmuebleArrendamiento
                        // Vamos a identificar porque la tabla NOUTILI esta incompleta, primero vamos a corregir NOUTILI.
                        // Es necesario identificar el numero minimo de condiciones para corregir el problema.
                        // Lo que debe de ocurrir es que este programa deje de encontrar registros que caigan en esta parte.
                        // vamos a tratar de reconstruir la relación via los datos del inmueble
                        // incompleto contiene los contratos vigentes que estan en la base y no tienen un inmueble localizable 
                        var incompleto = (from contrato in Contratos
                                          where vigente.FolioContrato == contrato.FolioContratoArrto
                                          select new
                                          {
                                              FolioContrato = contrato.FolioContratoArrto,
                                              IdContrato = contrato.IdContratoArrto,
                                              Inst = contrato.Fk_IdInstitucion,
                                              FechaFin = contrato.FechaFinOcupacion,
                                              FechaFinVigente = vigente.FechaContratoHasta
                                          });
                        LOGFile.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                            vigente.CodigoPostal);
                        // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                        // que los folios de contrato son iguales
                        foreach (var r in incompleto)
                        {
                            DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                            new DateTime(1990, 1, 1);
                            DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                            new DateTime(1990, 1, 1);
                            LOGFile.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                                r.Inst + "-" +
                                fecha + "-" + fechaVigente);

                            // 
                            // Vamos a buscar el inmueble por calle y numero exterior usando el idinmueble del contrato y comparando la
                            // direccion del inmueble y del contrato vigente
                            // si funciona con el IdInmueble buscaremos el contrato para tratar de cuadrar información
                            // Puede ocurrir que haya correcciones manuales en los nombres de las calles
                            var busqInm = (from inmueble in inmuebles
                                           where vigente.Calle.Replace("á", "a")
                                                              .Replace("é", "e")
                                                              .Replace("í", "i")
                                                              .Replace("ó", "o")
                                                              .Replace("ú", "u")
                                                              .Replace("Á", "A")
                                                              .Replace("É", "E")
                                                              .Replace("Í", "I")
                                                              .Replace("Ó", "O")
                                                              .Replace("Ú", "U") ==
                                                 inmueble.NombreVialidad.Replace("á", "a")
                                                              .Replace("é", "e")
                                                              .Replace("í", "i")
                                                              .Replace("ó", "o")
                                                              .Replace("ú", "u")
                                                              .Replace("Á", "A")
                                                              .Replace("É", "E")
                                                              .Replace("Í", "I")
                                                              .Replace("Ó", "O")
                                                              .Replace("Ú", "U") &
                                              vigente.NumExterior == inmueble.NumExterior &
                                              vigente.CodigoPostal == inmueble.CodigoPostal
                                           select inmueble);
                            switch(busqInm.Count())
                            {
                                case 0:
                                    LOGFile.WriteLine("No se encontró in inmueble para el contrato vigente " + vigente.ID);
                                    break;
                                case 1:
                                    LOGFile.WriteLine("Candidato");
                                    foreach (var inmueb in busqInm)
                                    {
                                        var regNoUtil = (from relinv in NoUtili
                                                         where vigente.Calle == relinv.NombreVialidad &
                                                               vigente.NumExterior == relinv.NumExterior &
                                                               vigente.CodigoPostal == relinv.CodigoPostal.ToString()
                                                         select new
                                                         {
                                                             FolioContrato = vigente.FolioContrato,
                                                             IDNoUtili = relinv.Identificador
                                                         });
                                        LOGFile.WriteLine("-Inmueble " + inmueb.IdInmuebleArrendamiento + "-" +
                                            inmueb.NombreVialidad + "-" +
                                            inmueb.NumExterior + "-" +
                                            inmueb.CodigoPostal + "- regs " + regNoUtil.Count());
                                        switch (regNoUtil.Count())
                                        {
                                            case 0:
                                                LOGFile.WriteLine("No se encontro un registro NoUtili para este contrato");
                                                break;
                                            case 1:
                                                foreach (var rn in regNoUtil)
                                                {
                                                    LOGFile.WriteLine(" NoUtli record " + rn.IDNoUtili);
                                                    SqlFile.WriteLine("Update NoUtiliAuditoria2019T set FolioContratoArrto = " + r.FolioContrato +
                                                    ", IdInmuebleArrendamiento = " + inmueb.IdInmuebleArrendamiento +
                                                    " where Identificador = " + rn.IDNoUtili);
                                                }
                                                break;
                                            default:
                                                LOGFile.WriteLine("Encontramos mas de un registro el NoUtili **** O J O");
                                                foreach (var rn in regNoUtil)
                                                {
                                                    LOGFile.WriteLine(" NoUtli record " + rn.IDNoUtili);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    LOGFile.WriteLine("Encontramos mas de un candidato");
                                    foreach (var inmueb in busqInm)
                                    {
                                        LOGFile.WriteLine("-Inmueble " + inmueb.IdInmuebleArrendamiento + "-" + inmueb.NombreVialidad + "-" +
                                            inmueb.NumExterior + "-" + inmueb.CodigoPostal);
                                    }
                                    break;
                            }
                        }
                    }
                }
                
                //trans.Commit();
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                LOGFile.WriteLine("Error al actualizar NoUtili " + ex.ToString());
            }
            LOGFile.WriteLine("Registros NoUtili modificados " + NoUtiliRegsModified);
            LOGFile.WriteLine("Contratos sin Inmueble " + ContratosSinInmueble);
            SqlFile.Close();
            LOGFile.Close(); 
        }

        static public string GetPais(int FkIdPais)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_Pais where FkIdPais == pay.IdPais select pay);
            foreach (var p in Pais) return p.DescripcionPais;
            return "";
        }

        static public string GetEstado(int FkIdEstado)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_Estado where FkIdEstado == pay.IdEstado select pay);
            foreach (var p in Pais) return p.DescripcionEstado;
            return "";
        }

        static public string GetMunicipio(int FkIdMunicipio)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_Municipio where FkIdMunicipio == pay.IdMunicipio select pay);
            foreach (var p in Pais) return p.DescripcionMunicipio;
            return "";
        }

        static public string GetTipoInmueble(int FkIdTipoInmueble)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_TipoInmueble where FkIdTipoInmueble == pay.IdTipoInmueble select pay);
            foreach (var p in Pais) return p.DescripcionTipoInm;
            return "";
        }

        static public string GetTipoUsoInmueble(int FkIdTipoUsoInmueble)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_TipoUsoInmueble where FkIdTipoUsoInmueble == pay.IdTipoUsoInm select pay);
            foreach (var p in Pais) return p.DescripcionTipoUsoInm;
            return "";
        }

        static public string GetInstitucion(int FkIdInstitucion)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Institucion = (from inst in ctx.Cat_Institucion where FkIdInstitucion == inst.IdInstitucion select inst);
            foreach (var i in Institucion) return i.DescripcionInstitucion;
            return "";
        }

        static public bool Result(string linein, out string line, bool comp, string msg)
        {
            line = linein;
            if (comp)
            {
                line += "1";
                return true;
            }
            else
            {
                if (msg.Equals("")) line += "0"; else line += msg;
                return false;
            }
        }

        /*
         * Todos los registros vigentes que proporcionó el usuario están en el Reporte Total
         */

        static void BusquedaVigentesEnReporteTotal()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var Inmuebles = ctx.InmuebleArrendamiento;
            var ReporteTotal = ctx.ReporteTotal;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";
            string LOGqqsaved = @"D:\temp\LOGqqsaved.txt";
            string line;
            int registrosErroneos = 1;
            int registros = 0;

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            if (File.Exists(LOGqqsaved)) File.Delete(LOGqqsaved);
            StreamWriter LOGFile = new StreamWriter(LOGqqsaved);
            LOGFile.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);
            
            bool RegistrosIguales = true;
            foreach(var vigente in Vigentes)
            {
                registros++;
                line = ">" + vigente.ID.ToString() + "/" + vigente.FolioContrato.ToString() + "<";
                var totalEncontrados = (from total in ReporteTotal
                                        where total.FolioContrato == vigente.FolioContrato
                                        select total);
                foreach(var total in totalEncontrados)
                {
                    RegistrosIguales = true;
                                                        //  1
                    RegistrosIguales &= Result(line, out line, vigente.FolioContrato == total.FolioContrato, "");
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Propietario).Equals(U.EDS(total.Propietario)), "");
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Responsable).Equals(U.EDS(total.Responsable)), "");
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Promovente).Equals(U.EDS(total.Promovente)), "");
                    string pais = GetPais(total.FkIdPais).ToUpper();
                                                        //  5
                    RegistrosIguales &= Result(line, out line, vigente.Pais.Equals(pais), ""); // no letra y numero
                    //if (vigente.Estado + "-" + total.Estado); // total vacio
                    string estado = GetEstado((int)total.Fk_IdEstado).ToUpper();
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Estado).Equals(U.EDS(estado)) || U.IsExcelNull(estado), "-Edo no vacio-");
                    //if (vigente.Municipio + "-" + total.Municipio); // total vacio
                    string municipio = GetMunicipio((int)total.Fk_IdMunicipio).ToUpper();
                    if(!(U.EDS(vigente.Municipio).Equals(U.EDS(municipio)) || U.IsExcelNull(municipio)))
                    {
                        SqlFile.WriteLine("{0}-{1}/{2}-{3}-{4}",total.FolioContrato,vigente.ID,total.Fk_IdMunicipio,municipio, vigente.Municipio);
                    }
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Municipio).Equals(U.EDS(municipio)) || U.IsExcelNull(municipio), "-Municipio no vacio-");
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Colonia).Equals(U.EDS(total.Colonia)) || U.IsExcelNull(total.Colonia), "");
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.Calle).Equals(U.EDS(total.Calle)), "");
                                                        //  10
                    RegistrosIguales &= Result(line, out line, vigente.CodigoPostal.Equals(total.CodigoPostal), "");
                    if (!U.EDS(vigente.NumInterior).Equals(U.EDS(total.NumInterior)))
                    {
                        SqlFile.WriteLine("{0}-{1}/{2}-{3}-{4}", total.FolioContrato, vigente.ID, total.Fk_IdMunicipio, municipio, vigente.Municipio);
                    }
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.NumInterior).Equals(U.EDS(total.NumInterior)), ""); // sin info los dos
                    
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.NumExterior).Equals(U.EDS(total.NumExterior)), "Numero ext dif");
                    RegistrosIguales &= Result(line, out line, (U.IsExcelNull(vigente.Ciudad) && U.IsExcelNull(total.Ciudad)) || vigente.Ciudad.Equals(total.Ciudad), ""); // nulo blanco
                                // O J  O
                    // Otro uso inmueble: preguntar si es importante o quen gana o como se define
                    RegistrosIguales &= Result(line, out line, vigente.OtroUsoInmueble.Equals(total.OtroUsoInmueble) || true, "");
                    
                                                        //  15
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.TipoContrato).Equals(U.EDS(total.TipoContrato)), "");
                    RegistrosIguales &= Result(line, out line, (U.IsExcelNull(vigente.TipoOcupacion) && U.IsExcelNull(total.TipoOcupacion)) || vigente.TipoOcupacion.Equals(total.TipoOcupacion), ""); // vacio NULL
                    RegistrosIguales &= Result(line, out line, vigente.DescripcionTipoArrendamiento.Equals(total.DescripcionTipoArrendamiento), "");
                    //if (vigente.TipoInmueble + "-" + total.TipoInmueble); // total sin info
                    string tipoInmueble = GetTipoInmueble((int)total.Fk_IdTipoInmueble).ToString();
                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.TipoInmueble).Equals(U.EDS(tipoInmueble)), "-TipoInmueble no vacio-");
                    //if (vigente.TipoUsoInmueble + "-" + total.TipoUsoInmueble); // total sin info
                    string tipoUsoInmueble = GetTipoUsoInmueble((int)total.Fk_IdTipoUsoInmueble).ToString();

                    RegistrosIguales &= Result(line, out line, U.EDS(vigente.TipoUsoInmueble).Equals(U.EDS(tipoUsoInmueble)) || U.IsExcelNull(tipoUsoInmueble), "-TipoUsoInmueble no vacio-");
                                                        //  20
                    RegistrosIguales &= Result(line, out line, vigente.AreaOcupadaM2 == total.AreaOcupadaM2, "");
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.MontoPagoMensual == total.MontoPagoMensual, "");
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.CuotaMantenimiento == total.CuotaMantenimiento, "");
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.MontoPagoPorCajonesEstacionamiento == total.MontoPagoPorCajonesEstacionamiento, "");
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.MontoDictaminado == total.MontoDictaminado, "");
                                                        //  25
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.RentaUnitariaMensual == total.RentaUnitariaMensual, "");
                    RegistrosIguales &= Result(line, out line, (decimal)vigente.MontoAnterior == total.MontoAnterior, "");
                    RegistrosIguales &= Result(line, out line, vigente.SMOI == total.SMOI, "");
                    RegistrosIguales &= Result(line, out line, vigente.TablaSmoi.Equals(total.TablaSMOI), "");
                    RegistrosIguales &= Result(line, out line, (DateTime)vigente.Fecha == Convert.ToDateTime(total.Fecha), "");
                                                        //  30
                    RegistrosIguales &= Result(line, out line, (DateTime)vigente.FechaContratoDesde == Convert.ToDateTime(total.FechaContratoDesde), "");
                    RegistrosIguales &= Result(line, out line, (DateTime)vigente.FechaContratoHasta == Convert.ToDateTime(total.FechaCntratoHasta), "");
                    RegistrosIguales &= Result(line, out line, (DateTime)vigente.FechaDictamen == Convert.ToDateTime(total.FechaDictamen), "");
                    RegistrosIguales &= Result(line, out line, (U.IsExcelNull(vigente.DescripcionTipoContratacion) && U.IsExcelNull(total.DescripcionTipoContratacion)) || U.EDS(vigente.DescripcionTipoContratacion).Equals(U.EDS(total.DescripcionTipoContratacion)), "");
                    RegistrosIguales &= Result(line, out line, vigente.ResultadoOpinion.Equals(total.ResultadosOpinion), "");
                                                        //  35
                    RegistrosIguales &= Result(line, out line, vigente.RIUF.Equals(total.RIUF), "");
                    if (!RegistrosIguales) 
                        LOGFile.WriteLine((registrosErroneos++).ToString() + line);
                }
            }
            
            LOGFile.WriteLine("Registros erroneos " + (registrosErroneos - 1).ToString());
            Console.WriteLine("Registros Erroneo " + (registrosErroneos - 1).ToString());
            Console.WriteLine("Registros " + (registros).ToString());
            SqlFile.Close();
            LOGFile.Close();
        }

        /*
         No Encontré la forma de hacer jalar esta forma de hacer las cosas

        // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/how-to-call-custom-database-functions
        
        [EdmFunction("ArrendemientoInmuebleModel.Store", "EDS")]
        public static decimal AvgStudentGrade(int stid)
        {
            throw new NotSupportedException("Direct calls to EDS are not supported.");
        }
        */

        static void ReporteTotalEnInmuebles()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var Inmuebles = ctx.InmuebleArrendamiento;
            var ReporteTotal = ctx.ReporteTotal;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";
            string LOGqqsaved = @"D:\temp\LOGqqsaved.txt";
            string line;
            int registrosErroneos = 1;
            int registros = 0;

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            if (File.Exists(LOGqqsaved)) File.Delete(LOGqqsaved);
            StreamWriter LOGFile = new StreamWriter(LOGqqsaved);
            LOGFile.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);

            bool RegistrosIguales;
            foreach (var reporte in ReporteTotal)
            {
                registros++;
                
                line = ">" + reporte.NumeroSecuencial.ToString() + "/" + reporte.FolioContrato.ToString() + "<";

                // No podemos usar LINQ para llamar funciones creadas en c#
                // manejemos la memoria de la computadora local
                /*
                var inmueblesEncontrados = (from inmueble in Inmuebles
                                            where EDS(reporte.Calle).Equals(EDS(inmueble.NombreVialidad))
                                            select new { AvgStudentGrade("hola") });
                */
                foreach (var inmueble in Inmuebles)
                {
                    if (U.EDS(reporte.Calle).Equals(U.EDS(inmueble.NombreVialidad))) { // esto es casi equivalente a la expr LINQ
                        RegistrosIguales = true;
                        line += ">" + inmueble.IdInmuebleArrendamiento.ToString() + "<";
                        //  1
                        RegistrosIguales &= Result(line, out line, reporte.FkIdPais == inmueble.Fk_IdPais, "-pais no coincide-" + reporte.FkIdPais.ToString() + "-" + inmueble.Fk_IdPais.ToString() + "-");
                        //RegistrosIguales &= Result(line, out line, GetInstitucion((int)reporte.Fk_IdInstitucion).Equals(GetInstitucion((int)inenc.Fk_IdInstitucion)), "-institucion no coincide-");
                        RegistrosIguales &= Result(line, out line, reporte.Fk_IdTipoInmueble == inmueble.Fk_IdTipoInmueble, "-institucion no coincide-" + reporte.Fk_IdTipoInmueble.ToString() + "-" + inmueble.Fk_IdTipoInmueble.ToString() + "-");
                        RegistrosIguales &= Result(line, out line, reporte.Fk_IdEstado == inmueble.Fk_IdEstado, "-Estado no coincide-" + reporte.Fk_IdEstado.ToString() + "-" + inmueble.Fk_IdEstado.ToString() + "-");
                        RegistrosIguales &= Result(line, out line, reporte.Fk_IdMunicipio == inmueble.Fk_IdMunicipio, "-Municipio no coincide-" + reporte.Fk_IdMunicipio.ToString() + "-" + inmueble.Fk_IdMunicipio.ToString() + "-");
                        RegistrosIguales &= Result(line, out line, reporte.Fk_IdLocalidad == inmueble.Fk_IdLocalidad, "-Localidad no coincide-" + reporte.Fk_IdLocalidad.ToString() + "-" + inmueble.Fk_IdLocalidad.ToString() + "-");
                        //  5
                        RegistrosIguales &= Result(line, out line, U.EDS(reporte.Colonia).Equals(U.EDS( inmueble.OtraColonia)), "-Colonia no coincide-" + U.IsNull(reporte.Colonia).ToString() + "-" + U.IsNull(inmueble.OtraColonia).ToString() + "-");
                        RegistrosIguales &= Result(line, out line, U.EDS(reporte.Calle).Equals(U.EDS(inmueble.NombreVialidad)), "-Calle no coincide-" + U.IsNull(reporte.Calle).ToString() + "-" + U.IsNull(inmueble.NombreVialidad).ToString() + "-");
                        RegistrosIguales &= Result(line, out line, U.EDS(reporte.NumExterior).Equals(U.EDS(inmueble.NumExterior)), "-NumExterior no coincide-" + U.IsNull(reporte.NumExterior) + "-" + U.IsNull(inmueble.NumExterior) + "-");
                        RegistrosIguales &= Result(line, out line, U.EDS(reporte.NumInterior).Equals(U.EDS(inmueble.NumInterior)), "-NumInterior no coincide-" + U.IsNull(reporte.NumInterior) + "-" + U.IsNull(inmueble.NumInterior) + "-");
                        RegistrosIguales &= Result(line, out line, U.EDS(reporte.CodigoPostal).Equals(U.EDS(inmueble.CodigoPostal)), "-Codigo Postal no coincide-" + U.IsNull(reporte.CodigoPostal) + "-" + U.IsNull(inmueble.CodigoPostal) + "-");

                        if (!RegistrosIguales)
                            LOGFile.WriteLine((registrosErroneos++).ToString() + line);
                    }
                }
            }

            LOGFile.WriteLine("Registros erroneos " + (registrosErroneos - 1).ToString());
            Console.WriteLine("Registros Erroneo " + (registrosErroneos - 1).ToString());
            Console.WriteLine("Registros " + (registros).ToString());
            SqlFile.Close();
            LOGFile.Close();
        }

        static void Main(string[] args)
        {
            //comparacionNoUtili(); // No funcionó
            //BusquedaVigentes();   // No funcionó

            //BusquedaVigentesEnReporteTotal();  // Funcionó - Todo vigente está en Reporte Total
            //Hay que probar si Reporte Total está en inmuebles
            ReporteTotalEnInmuebles();
        }
    }
}

/*
                       // Existia la posibilidad de que el numero de folio de contrato fuera un IdContrato, pero en muchos casos
                       // No apunta a ningún lado con información válida
                       var contXIDEncontrato = (from relacionInv in NoUtili
                                             where r.IdContrato == relacionInv.FolioContratoArrto
                                             select relacionInv);
                       if (contXIDEncontrato.Count() > 0)
                       {
                           LOGFile.WriteLine("Datos encontrados para verificar");
                           foreach (var rIDE in contXIDEncontrato)
                           {
                               DateTime fechaID = rIDE.FechaFinOcupacion != null ? (DateTime)rIDE.FechaFinOcupacion :
                                   new DateTime(1990, 1, 1);
                               LOGFile.WriteLine("-FolioContrato " + rIDE.FolioContratoArrto + "-" + fechaID + "-" +
                                   rIDE.IdInmuebleArrendamiento + "-" + rIDE.Fk_IdInstitucion + "-" + rIDE.NombreVialidad + "-" + rIDE.NumExterior);
                           }
                       }
                       else
                       {
                           LOGFile.WriteLine("- Sin datos");
                       }
                       */

/*
            foreach(NoUtiliAuditoria2019T renglonTest in tablaTest)
            {
                DateTime fecha = renglonTest.FechaFinOcupacion != null ? (DateTime) renglonTest.FechaFinOcupacion :
                    new DateTime(1990,1,1);
                LOGFile.WriteLine("Test " + renglonTest.FolioContratoArrto + " " +
                                            renglonTest.IdInmuebleArrendamiento + " " +
                                            renglonTest.Fk_IdInstitucion + " " +
                                            fecha.ToString("yyyy MMMM dd"));
                
            }
            */

/*
 // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
                    // porque el folio del vigente no esta en la tabla NOUTILI, se verifico los contratos vigentes todos tienen folio Contrato
                    // Vamos a identificar porque la tabla NOUTILI esta incompleta, primero vamos a corregir NOUTILI.
                    // Es necesario identificar el numero minimo de condiciones para corregir el problema.
                    // Lo que debe de ocurrir es que este programa deje de encontrar registros que caigan en esta parte.
                    // vamos a tratar de reconstruir la relación via los datos del inmueble
                    // incompleto contiene los contratos vigentes que estan en la base y no tienen un inmueble localizable 
                    var incompleto = (from relinv in NoUtili
                                      where vigente.Calle == relinv.NombreVialidad &
                                            vigente.NumExterior == relinv.NumExterior &
                                            vigente.CodigoPostal == relinv.CodigoPostal.ToString()
                                      select new
                                      {
                                          FolioContrato = vigente.FolioContrato,
                                          IDNoUtili = relinv.Identificador
                                      });
                    LOGFile.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                        vigente.CodigoPostal);
                    // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                    // que los folios de contrato son iguales
                    if (incompleto.Count() == 1)
                    {
                        foreach (var r in incompleto)
                        {
                            NoUtiliRegsModified++;
                            LOGFile.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                            SqlFile.WriteLine("Update NoUtiliAuditoria2019T set FolioContratoArrto = " + r.FolioContrato + " where Identificador = " + r.IDNoUtili); 
                        }
                    }
                    else
                    {
                        registrosIrreparables++;
                        LOGFile.WriteLine("0 o varios registros noutili que cumplen la condición");
                        foreach (var r in incompleto)
                        {
                            LOGFile.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                        }
                    }
 */
/*    Ejemplo de como registrar funciones en sql
CREATE ASSEMBLY [MyAssembly] 
FROM 'C:\Path\To\Assembly\MyAssembly.dll'
WITH PERMISSION_SET = SAFE
GO

CREATE FUNCTION [dbo].[Replace](@input [nvarchar](4000), @pattern [nvarchar](4000),  @replacement [nvarchar](4000), @options [int] = 0)
  RETURNS [nvarchar](4000) NULL
  WITH EXECUTE AS CALLER
  AS EXTERNAL NAME [MyAssembly].[MyNamespace.UserDefinedFunctions].[Replace]
GO
*/
