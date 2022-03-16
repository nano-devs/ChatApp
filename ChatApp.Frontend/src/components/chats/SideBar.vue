<template>
  <v-navigation-drawer
    app
    mobile-breakpoint="766"
    v-model="drawer"
    :permanent="!$vuetify.breakpoint.mdAndDown"
  >
    <v-sheet color="grey lighten-4" class="pa-4">
      <v-avatar class="mb-4" color="grey darken-1" size="64"></v-avatar>

      <div>
        Your ID:
        <span class="font-weight-bold">bX4Jyg</span>
      </div>
    </v-sheet>

    <v-divider class="py-0"></v-divider>

    <v-subheader>
      Your contacts
      <v-spacer></v-spacer>
      <v-btn icon>
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-subheader>

    <v-list class="py-0" nav dense shaped>
      <v-list-item-group v-model="group" active-class="indigo--text">
        <template v-for="(contact, index) in contacts">
          <v-list-item
            link
            @click="selectedContact = contact"
            :to="'/dm/' + contact.name"
            replace
            :key="index"
          >
            <v-list-item-avatar color="grey darken-1"></v-list-item-avatar>

            <v-list-item-content>
              <v-list-item-title>{{ contact.name }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </template>
      </v-list-item-group>
    </v-list>

    <template v-slot:append>
      <v-sheet color="grey lighten-4">
        <v-divider></v-divider>
        <div class="pa-2">
          <v-btn block color="red" dark>Logout</v-btn>
        </div>
      </v-sheet>
    </template>
  </v-navigation-drawer>
</template>

<script>      
export default {
  data: () => ({
    contacts: [
      { name: 'Contact 1' },
      { name: 'Contact 2' },
      { name: 'Contact 3' },
      { name: 'Contact 4' },
      { name: 'Contact 5' },
      { name: 'Contact 6' },
    ],
    drawer: null,
    data: null,
    group: null,
    messages: [],
    selectedContact: null,
  }),
  mounted: function () {
    this.selectedContact = this.contacts[0];
  },
  methods: {
    toggleDrawer() {
      this.drawer = !this.drawer
    }
  }
};
</script>
