namespace Grupo_negro.Configuration
{
    public static class FeatureFlags
    {
        // Control de funcionalidades - TODAS ACTIVADAS PARA DEMOSTRACIÓN
        // Sistema de saldo y transacciones
        public static bool SISTEMA_SALDO_HABILITADO = true;
        public static bool DEPOSITOS_RETIROS_HABILITADO = true;
        public static bool PANEL_ADMIN_HABILITADO = true;
        public static bool INTEGRACION_SALDO_APUESTAS_HABILITADO = true;
        public static bool RESOLUCION_APUESTAS_HABILITADO = true;
        public static bool ROLES_SISTEMA_HABILITADO = true;
        
        // Funcionalidades siempre activas (base del sistema)
        public static bool SISTEMA_APUESTAS_BASE = true;
        public static bool AUTENTICACION_BASICA = true;
    }
}