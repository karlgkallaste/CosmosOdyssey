import {createApp} from 'vue'
// import './style.css'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Tag from "primevue/tag";
import Select from "primevue/select";
import Button from "primevue/button";
import Menubar from "primevue/menubar";
import AccordionPanel from "primevue/accordionpanel";
import Accordion from "primevue/accordion";
import AccordionHeader from "primevue/accordionheader";
import AccordionContent from "primevue/accordioncontent";
import Dialog  from "primevue/dialog";
import InputText  from "primevue/inputtext";
import Message  from "primevue/message";
import Timeline  from "primevue/timeline";
import Divider  from "primevue/divider";
import DatePicker  from "primevue/datepicker";
import Toast  from "primevue/toast";
import FloatLabel  from "primevue/floatlabel";
import router from "../router/router.ts";
import ToastService from 'primevue/toastservice';
import './tailwind.css';
import 'primeicons/primeicons.css';

createApp(App)
    .use(router)
    .use(PrimeVue, {
        theme: {
            preset: Aura,
            options: {
                cssLayer: {
                    name: 'primevue',
                    order: 'tailwind-base, primevue, tailwind-utilities'
                }
            }
        }
    })
    .use(ToastService)
    .component('DataTable', DataTable)
    .component('Column', Column)
    .component('Tag', Tag)
    .component('Select', Select)
    .component('Button', Button)
    .component('Menubar', Menubar)
    .component('AccordionPanel', AccordionPanel)
    .component('Accordion', Accordion)
    .component('AccordionHeader', AccordionHeader)
    .component('AccordionContent', AccordionContent)
    .component('Dialog', Dialog)
    .component('InputText', InputText)
    .component('Message', Message)
    .component('Timeline', Timeline)
    .component('Divider', Divider)
    .component('Calendar', DatePicker)
    .component('Toast', Toast)
    .component('FloatLabel', FloatLabel)
    .mount('#app')
