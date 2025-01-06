﻿import { createRouter, createWebHistory } from "vue-router";
import Home from "../src/views/Home.vue"; // Example of a view/component import
import About from "../src/views/About.vue"; // Example of a view/component import
import Legs from "../src/views/Legs.vue"; // Example of a view/component import

// Define your routes
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
        path: '/legs/from:from/to:to',
        name: 'Legs',
        component: Legs,
        props: true
    },
];

// Create the router instance
const router = createRouter({
    history: createWebHistory(), // Use history mode
    routes,
});

export default router;