import '../styles/site.css';

import 'alpinejs';
import axios from 'axios';

import { library, dom } from "@fortawesome/fontawesome-svg-core";
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';

library.add(fas, far, fab);
dom.watch();

declare var apiHost: string;

interface Event {
    id: number,
    title: string,
    date: Date
}

export function setupNav() {
    return {
        toggleMenu() {
            var headerNav = document.getElementById("headerNav");
            if (headerNav) {
                if (headerNav.classList.contains("hidden")) {
                    headerNav.classList.remove("hidden");
                } else {
                    headerNav.classList.add("hidden");
                }
            }
        }
    }
}

export function setupEvents() {
    return {
        events: [] as Event[],
        async mounted() {
            await this.loadEvents();
        },
        async deleteEvent(currentEvent: Event) {
            if (confirm(`Are you sure you want to delete ${currentEvent.title}`)) {
                await axios.delete(`${apiHost}/api/events/${currentEvent.id}`);
                await this.loadEvents();
            }
        },
        async loadEvents() {
            try {
                const response = await axios.get(`${apiHost}/api/events`);
                this.events = response.data;
            } catch (error) {
                console.log(error);
            }
        }
    }
}

export function createOrUpdateEvent() {
    return {
        event: {} as Event,
        async create() {
            try {
                this.event.date = new Date(this.event.date);
                await axios.post(`${apiHost}/api/events`, this.event);
                window.location.href="/events";
            } catch (error) {
                console.log(error);
            }
        },
        async update() {
            try {
                this.event.date = new Date(this.event.date);  //2021-05-13 5/13/2021
                await axios.put(`${apiHost}/api/events/${this.event.id}`, this.event);
                window.location.href="/events";
            } catch (error) {
                console.log(error);
            }
        },
        async loadData() {
            const pathnameSplit = window.location.pathname.split('/');
            const id = pathnameSplit[pathnameSplit.length - 1];
            try {
                const response = await axios.get(`${apiHost}/api/events/${id}`);
                this.event = response.data;
            } catch (error) {
                console.log(error);
            }
        }
    }
}

export function toggleMenuWithJs() {
    var headerNav = document.getElementById("headerNav");
    if (headerNav) {
        if (headerNav.classList.contains("hidden")) {
            headerNav.classList.remove("hidden");
        } else {
            headerNav.classList.add("hidden");
        }
    }
}