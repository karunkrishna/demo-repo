from django.http import HttpResponse

def index(request):
    return HttpResponse("<h2> This is the response to the music app homepage")