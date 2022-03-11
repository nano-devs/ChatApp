<template>
  <v-main>

    <v-divider></v-divider>

    <v-footer app color="transparent" inset class="pb-6">
      <v-textarea
        background-color="grey"
        flat
        auto-grow
        solo
        rounded
        dark
        hide-details
        rows="1"
        append-icon="mdi-emoticon"
        prepend-inner-icon="mdi-plus-circle"
      ></v-textarea>
    </v-footer>
  </v-main>
</template>

<script>
// import * as signalR from "@microsoft/signalr";

export default {
  data: () => ({
    drawer: null,
    data: null,
    messages: [],
  }),
  created: function () {
    // let thisVue = this;

    // this.connection = new signalR.HubConnectionBuilder()
    //   .withUrl("https://localhost:7238/chathub", {
    //     skipNegotiation: true,
    //     transport: signalR.HttpTransportType.WebSockets,
    //   })
    //   .configureLogging(signalR.LogLevel.Information)
    //   .build();

    // this.connection.on("ReceiveMessage", function (user, message) {
    //   thisVue.messages.push({ user, message });
    //   console.log({ user, message });
    // });

    // this.connection.start();
  },
};
</script>
