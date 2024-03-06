namespace AGENDA
{
    internal class Contacto
    {
        #region Snippets
        /// <summary>
        /// limpia el texto de acentos y mayúsculas
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        private static string LimpiarTexto(string cadena)
        {
            cadena = cadena.ToLower();
            if (cadena.Contains("á")) cadena = cadena.Replace("á", "a");
            if (cadena.Contains("é")) cadena = cadena.Replace("é", "e");
            if (cadena.Contains("í")) cadena = cadena.Replace("í", "i");
            if (cadena.Contains("ó")) cadena = cadena.Replace("ó", "o");
            if (cadena.Contains("ú")) cadena = cadena.Replace("ú", "u");
            if (cadena.Contains("ü")) cadena = cadena.Replace("ü", "u");

            return cadena;
        }
        /// <summary>
        /// carga el fichero de la agenda en una lista de arrays de string
        /// </summary>
        /// <param name="rutaFichero"></param>
        /// <returns></returns>
        private static List<string[]> cargarFicheroAgenda(string rutaFichero)
        {
            List<string[]> agenda = new();
            string[] entrada;
            using (StreamReader ficheroLectura = new(rutaFichero))
            {
                while (!ficheroLectura.EndOfStream)
                {
                    entrada = ficheroLectura.ReadLine().Split(";");
                    agenda.Add(entrada);
                }
            }
            return agenda;
        }
        /// <summary>
        /// muestra un contacto calculando el espaciado para que quede alineado
        /// </summary>
        /// <param name="contacto"></param>
        private static void MostarDatos(string[] contacto)
        {
            string espaciado = new string(' ', 25 - contacto[0].Length);
            Console.WriteLine($"Nombre: {contacto[0]}{espaciado}Teléfono: {contacto[1]}");
        }
        #endregion

        #region Funciones del Menu
        public static void Despedida() { Console.WriteLine("ADIOS"); }
        public static void Lista(string rutaFichero)
        {
            List<string[]> agenda = cargarFicheroAgenda(rutaFichero);
            if (agenda.Count == 0) //Si no hay contactos, muestra un mensaje
            {
                Console.WriteLine("No hay contactos en la agenda");
                return;
            }
            else
            {
                /* La función flecha (lambda) es una función anónima, se usa para
                 * simplificar el código.*/
                agenda.Sort((x, y) => string.Compare(x[0], y[0]));
                //Dados dos elementos x e y, compara el primer campo de x con el primer campo de y

                foreach (string[] contacto in agenda)//Muestra la lista ordenada
                {
                    MostarDatos(contacto);
                }
            }
        }
        public static void Crear(string rutaFichero)
        {
            List<string[]> agenda = cargarFicheroAgenda(rutaFichero);
            string nombre;
            string telefono;
            int tel;
            bool existe = false;

            Console.Write("Introduce el nombre del contacto: ");
            nombre = Console.ReadLine().Trim();
            Console.Write("Introduce el teléfono del contacto: ");
            telefono = Console.ReadLine().Trim();

            if (int.TryParse(telefono, out tel) && telefono.Length == 9 && nombre.Length >= 3) //Comprueba que el nombre y el teléfono sean válidos
            {
                foreach (string[] contacto in agenda)//busca si el teléfono ya existe
                {
                    if (contacto[1] == telefono)//Si ya existe, muestra un mensaje de error y sale
                    {
                        Console.WriteLine("El teléfono ya existe en la agenda");
                        existe = true;
                        return;
                    }
                }
                if (!existe)//Si no existe, lo añade al fichero
                {
                    StreamWriter ficheroEscritura = new StreamWriter(rutaFichero, true, System.Text.Encoding.Default);
                    ficheroEscritura.WriteLine($"{nombre};{telefono}");
                    ficheroEscritura.Close();
                    Console.WriteLine("Contacto creado");
                }

            }
            else
            {
                if (nombre.Length < 3) Console.WriteLine("El nombre no es válido");
                if (!int.TryParse(telefono, out tel) || tel.ToString().Length != 9) Console.WriteLine("El teléfono no es válido");
            }

        }
        public static void Borrar(string rutaFichero)
        {
            List<string[]> agenda = cargarFicheroAgenda(rutaFichero);
            string rutaFicheroBak = rutaFichero.Substring(0, rutaFichero.Length - 3) + "bak";
            string telefono;
            bool encontrado = false;

            Console.Write("Introduce el teléfono del contacto que quieres borrar: ");
            telefono = Console.ReadLine();

            //busco el teléfono en la agenda
            for (int i = 0; i < agenda.Count; i++)
            {
                if (agenda[i][1] == telefono)
                {
                    //si lo encuentro, pido confirmación para borrarlo
                    Console.WriteLine($"Seguro que quieres borrar el contacto: {agenda[i][0]} - {agenda[i][1]}(escribe 'si' para confirmar)");
                    if (Console.ReadLine() == "si")
                    {
                        agenda.Remove(agenda[i]);
                        encontrado = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Se aborta el borrado a petición del usuario");
                        return;
                    }
                }
            }

            if (encontrado)
            {
                if (File.Exists(rutaFicheroBak)) File.Delete(rutaFicheroBak);//Si existe el fichero de copia de seguridad, lo borra
                File.Move(rutaFichero, rutaFicheroBak);//y creo la copia de seguridad
                using (StreamWriter ficheroEscritura = new StreamWriter(rutaFichero, false, System.Text.Encoding.Default))//Sobreescribo el fichero
                {
                    foreach (string[] contacto in agenda)
                    {
                        ficheroEscritura.WriteLine($"{contacto[0]};{contacto[1]}");
                    }
                }
                Console.WriteLine("Contacto borrado");//Mensaje de confirmación
            }
            else//Si no lo encuentra, se comunica al usuario
            {
                Console.WriteLine("El teléfono no existe en la agenda");
            }
        }
        public static void Buscar(string rutaFichero)
        {
            List<string[]> agenda = cargarFicheroAgenda(rutaFichero);
            string nombreABuscar;

            Console.Write("Dime el nombre a buscar: ");
            nombreABuscar = LimpiarTexto(Console.ReadLine());

            foreach (string[] contacto in agenda)
            {
                if (LimpiarTexto(contacto[0]).Contains(nombreABuscar))
                {
                    MostarDatos(contacto);
                }
            }
        }
        public static void Modificar(string rutaFichero)
        {
            List<string[]> agenda = cargarFicheroAgenda(rutaFichero);
            string rutaFicheroBak = rutaFichero.Substring(0, rutaFichero.Length - 3) + "bak";
            string telefono;
            bool cambiado = false;
            int indiceACambiar = -1; // Guardo el índice del contacto a cambiar lo inicilaizo a -1 para saber si no se ha encontrado el contacto
            string[] contactoCambiado = new string[2];

            Console.Write("Introduce el teléfono del contacto que quieres modificar: ");
            telefono = Console.ReadLine();

            //busco el teléfono en la agenda
            for (int i = 0; i < agenda.Count; i++)
            {
                if (agenda[i][1] == telefono)
                {
                    Console.WriteLine(agenda[i][0] + "-" + agenda[i][1]);
                    indiceACambiar = i; // Guardo el índice del contacto a cambiar
                }
            }
            if (indiceACambiar != -1)
            {
                Console.Write("Introduce el nombre modificado (deja vacio para mantener el actual): ");
                contactoCambiado[0] = Console.ReadLine();

                if (contactoCambiado[0].Length == 0) contactoCambiado[0] = agenda[indiceACambiar][0];
                contactoCambiado[1] = agenda[indiceACambiar][1];
                bool gatillo;
                do
                {
                    gatillo = false;
                    Console.Write("Introduce el teléfono modificado (deja vacio para mantener el actual): ");
                    contactoCambiado[1] = Console.ReadLine();
                    if (!int.TryParse(contactoCambiado[1], out _)) gatillo = true; //Si no es un número, repite el bucle
                    if (contactoCambiado[1].Length > 0 && contactoCambiado[1].Length != 9) gatillo=true;//Si no tiene 9 dígitos, repite el bucle
                    if (contactoCambiado[1].Length == 0) //Si no se ha introducido un teléfono, mantiene el actual
                    { 
                        contactoCambiado[1] = agenda[indiceACambiar][1]; 
                        gatillo = false;
                    }
                }
                while (gatillo);
                for (int i = 0; i < agenda.Count; i++)
                {
                    if (agenda[i][1] == contactoCambiado[1] && contactoCambiado[1] != agenda[indiceACambiar][1])
                       //Si el teléfono ya existe en la agenda y no es el mismo que el original
                    {
                        Console.WriteLine("El teléfono ya existe en la agenda");
                        return;
                    }
                }
                agenda.RemoveAt(indiceACambiar); // Elimino el contacto usando el índice
                agenda.Add(contactoCambiado);
                cambiado = true;
            }

            if (cambiado)
            {
                if (File.Exists(rutaFicheroBak)) File.Delete(rutaFicheroBak);//Si existe el fichero de copia de seguridad, lo borra
                File.Move(rutaFichero, rutaFicheroBak);//y creo la nueva copia de seguridad
                using (StreamWriter ficheroEscritura = new StreamWriter(rutaFichero, false, System.Text.Encoding.Default))//Sobreescribo el fichero
                {
                    foreach (string[] contacto in agenda)
                    {
                        ficheroEscritura.WriteLine($"{contacto[0]};{contacto[1]}");
                    }
                }
                Console.WriteLine("Contacto modificado");//Mensaje de confirmación

            }
            if (indiceACambiar == -1) Console.WriteLine("Ese contacto no existe en la agenda");
        }
        public static void BorrarTodos(string rutaFichero)
        {
            string rutaFicheroBak = rutaFichero.Substring(0, rutaFichero.Length - 3) + "bak";

            Console.WriteLine("Seguro que quieres borrar todos los contactos de la agenda ");
            Console.WriteLine("(escribe 'si' para confirmar)");
            if (Console.ReadLine() != "si")
            {
                Console.WriteLine("Se aborta el borrado a petición del usuario");
                return;
            }

            if (File.Exists(rutaFicheroBak)) File.Delete(rutaFicheroBak);//Si existe el fichero de copia de seguridad, lo borra
            File.Move(rutaFichero, rutaFicheroBak);//y creo la copia de seguridad
            File.Delete(rutaFichero);//Borro el fichero original
            Console.WriteLine("Todos los contactos han sido borrados");//Mensaje de confirmación
            StreamWriter ficheroEscritura = new StreamWriter(rutaFichero);
            ficheroEscritura.Close();

        }
        #endregion
    }
}
