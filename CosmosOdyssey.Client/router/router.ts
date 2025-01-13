﻿import {createRouter, createWebHistory} from "vue-router";
import Home from "../src/views/Home.vue";
import About from "../src/views/About.vue";
import Reservation from "../src/views/Reservation.vue";
import ReservationList from "../src/views/ReservationList.vue";
import Journey from "../src/views/Journey.vue";


const routes = [
    {
        path: "/",
        name: "Home",
        component: Home,
    },
    {
        path: '/about',
        name: 'About',
        component: About,
    },
    {
        path: '/journey',
        name: 'Journey',
        component: Journey,
        props: true
    },
    {
        path: '/reservation',
        name: 'Reservation',
        component: Reservation,
        props: true
    },
    {
        path: '/reservations',
        name: 'Reservations',
        component: ReservationList,
        props: true
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;