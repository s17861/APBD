import requests
import json


def print_res(res):
	print(f'{res.request.method.ljust(8)} - {res.status_code} {res.content if res.ok else ""}')


print_res(requests.get('http://localhost:55591/api/values'))
print_res(requests.get('http://localhost:55591/api/values/7'))
print_res(requests.post('http://localhost:55591/api/values', json='somevalue'))
print_res(requests.put('http://localhost:55591/api/values/13', json='othervalue'))
print_res(requests.delete('http://localhost:55591/api/values/14'))
