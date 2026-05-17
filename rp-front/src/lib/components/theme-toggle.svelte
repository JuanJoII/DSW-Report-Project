<script>
    import { onMount } from 'svelte';

    let isDark = $state(false);

    onMount(() => {
        // 1. Verificar preferencia guardada
        const savedTheme = localStorage.getItem('theme');
        // 2. Verificar preferencia del sistema
        const systemPrefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

        if (savedTheme === 'dark' || (!savedTheme && systemPrefersDark)) {
            isDark = true;
            document.documentElement.classList.add('dark');
        } else {
            isDark = false;
            document.documentElement.classList.remove('dark');
        }

        // Escuchar cambios en el sistema si no hay preferencia guardada
        const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        const handleChange = (e) => {
            if (!localStorage.getItem('theme')) {
                isDark = e.matches;
                if (e.matches) {
                    document.documentElement.classList.add('dark');
                } else {
                    document.documentElement.classList.remove('dark');
                }
            }
        };

        mediaQuery.addEventListener('change', handleChange);
        return () => mediaQuery.removeEventListener('change', handleChange);
    });

    function toggleTheme() {
        isDark = !isDark;
        if (isDark) {
            document.documentElement.classList.add('dark');
            localStorage.setItem('theme', 'dark');
        } else {
            document.documentElement.classList.remove('dark');
            localStorage.setItem('theme', 'light');
        }
    }
</script>

<button 
    onclick={toggleTheme}
    class="p-2 rounded-xl bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-300 hover:bg-violet-50 dark:hover:bg-violet-900/30 hover:text-sabana-violet dark:hover:text-sabana-lila transition-all shadow-sm border border-slate-200 dark:border-slate-700 flex items-center justify-center"
    title={isDark ? "Cambiar a modo claro" : "Cambiar a modo oscuro"}
>
    {#if isDark}
        <!-- Luna -->
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 3a6 6 0 0 0 9 9 9 9 0 1 1-9-9Z"/></svg>
    {:else}
        <!-- Sol -->
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="4"/><path d="M12 2v2"/><path d="M12 20v2"/><path d="m4.93 4.93 1.41 1.41"/><path d="m17.66 17.66 1.41 1.41"/><path d="M2 12h2"/><path d="M20 12h2"/><path d="m6.34 17.66-1.41 1.41"/><path d="m19.07 4.93-1.41 1.41"/></svg>
    {/if}
</button>
